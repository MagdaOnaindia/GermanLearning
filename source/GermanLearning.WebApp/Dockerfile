#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GermanLearning.WebApp/GermanLearning.WebApp.csproj", "GermanLearning.WebApp/"]
COPY ["GermanLearning.Application/GermanLearning.Application.csproj", "GermanLearning.Application/"]
COPY ["GermanLearning.Domain/GermanLearning.Domain.csproj", "GermanLearning.Domain/"]
COPY ["GermanLearning.PostgreInfrastructure/GermanLearning.PostgreInfrastructure.csproj", "GermanLearning.PostgreInfrastructure/"]
RUN dotnet restore "./GermanLearning.WebApp/GermanLearning.WebApp.csproj"
COPY . .
WORKDIR "/src/GermanLearning.WebApp"
RUN dotnet build "./GermanLearning.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GermanLearning.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GermanLearning.WebApp.dll"]