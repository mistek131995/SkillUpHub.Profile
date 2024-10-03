using SkillUpHub.Profile.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddCors(x => x.AddPolicy("CORS", builder =>
{
    builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
        .AllowCredentials();
}));

var app = builder.Build();

app.UseGrpcWeb();
app.UseCors();
app.MapGrpcService<ProfileService>().EnableGrpcWeb().RequireCors("CORS");

app.Run();