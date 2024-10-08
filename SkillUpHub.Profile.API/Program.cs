using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillHub.Profile.Application.Handlers;
using SkillUpHub.Profile.API.Middlewares;
using SkillUpHub.Profile.API.Services;
using SkillUpHub.Profile.Contract.Providers;
using SkillUpHub.Profile.Infrastructure.Clients;
using SkillUpHub.Profile.Infrastructure.Contexts;
using SkillUpHub.Profile.Infrastructure.Interfaces;
using SkillUpHub.Profile.Infrastructure.Providers;
using SkillUpHub.Profile.Infrastructure.Services;
using IServiceProvider = SkillHub.Profile.Application.Interfaces.IServiceProvider;
using ServiceProvider = SkillHub.Profile.Application.Providers.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<PGContext>(option => 
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepositoryProvider, RepositoryProvider>();
builder.Services.AddScoped<IServiceProvider, ServiceProvider>();

builder.Services.AddSingleton<IMessageBusClient, RabbitMqClient>(x =>
    new RabbitMqClient(builder.Configuration.GetSection("RabbitMqHost").Value));
builder.Services.AddScoped<IRabbitMqMessageHandler, RabbitMqMessageHandler>();

builder.Services.AddHostedService<RabbitMqListenerService>();

builder.Services.AddAuthorization();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>(); // Регистрируем Interceptor
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
        .AllowCredentials();
}));

var app = builder.Build();

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

// gRPC Web и Endpoints
app.UseGrpcWeb();
app.MapGrpcService<ProfileService>().EnableGrpcWeb().RequireCors("AllowAll");

app.Run();