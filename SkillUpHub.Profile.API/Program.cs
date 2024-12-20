using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillUpHub.Command.Application;
using SkillUpHub.Command.Infrastructure.Contexts;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Profile.API.Extensions;
using SkillUpHub.Profile.API.Middlewares;
using SkillUpHub.Query.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommands(builder.Configuration);
builder.Services.AddQueryServices(builder.Configuration);

#region Настройка аутентификации

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "SkillHub.Auth",
        ValidAudience = "SkillHub.Services",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SecretKey").Value)) // Ваш секретный ключ
    };

    // Важно: поддержка gRPC Web
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

#endregion

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName); // Используем полное имя типа
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Автоматическое применение миграций при старте приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PGContext>();
    dbContext.Database.Migrate();
}

// Включаем CORS до авторизации
app.UseCors("AllowAll");

// Аутентификация и авторизация
app.UseAuthentication();
app.UseAuthorization();

app.RegisterRoutes();
app.UseMiddleware<ExceptionHandlerMiddleware>();

var rabbitMqService = app.Services.GetService<IMessageBusClient>();
rabbitMqService!.Initialize();

app.Run();