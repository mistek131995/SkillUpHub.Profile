FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkillUpHub.Profile.API/SkillUpHub.Profile.API.csproj", "SkillUpHub.Profile.API/"]
COPY ["SkillUpHub.Profile.Application/SkillUpHub.Profile.Application.csproj", "SkillUpHub.Profile.Application/"]
COPY ["SkillUpHub.Profile.Contract/SkillUpHub.Profile.Contract.csproj", "SkillUpHub.Profile.Contract/"]
COPY ["SkillUpHub.Profile.Infrastructure/SkillUpHub.Profile.Infrastructure.csproj", "SkillUpHub.Profile.Infrastructure/"]
RUN dotnet restore "SkillUpHub.Profile.API/SkillUpHub.Profile.API.csproj"
COPY . .
WORKDIR "/src/SkillUpHub.Profile.API"
RUN dotnet build "SkillUpHub.Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkillUpHub.Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillUpHub.Profile.API.dll"]
