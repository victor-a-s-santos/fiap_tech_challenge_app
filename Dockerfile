FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 6000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./src/FIAP.TechChallenge.Api/FIAP.TechChallenge.Api.csproj", "./FIAP.TechChallenge.Api/"]
RUN dotnet restore "./FIAP.TechChallenge.Api/FIAP.TechChallenge.Api.csproj"
COPY . .
WORKDIR "/src/FIAP.TechChallenge.Api"
RUN dotnet build "FIAP.TechChallenge.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FIAP.TechChallenge.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.TechChallenge.Api.dll"]