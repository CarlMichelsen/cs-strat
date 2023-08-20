FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base

WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build

WORKDIR /src

COPY ["cs-strat.sln", "./"]

COPY . .

RUN dotnet restore

WORKDIR "/src/."

RUN dotnet restore "./Api"

RUN dotnet test

RUN dotnet build "./Api" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "./Api" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "./Api.dll"]
