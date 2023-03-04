FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app

COPY pandora-intelligence-assessment.sln .

COPY api/*.csproj ./api/
COPY tests/*.csproj ./tests/

RUN dotnet restore

COPY . .

RUN dotnet build -c Release

RUN dotnet test ./tests/tests.csproj

RUN dotnet publish ./api/api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build-env /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "api.dll", "--environment=Development"]
