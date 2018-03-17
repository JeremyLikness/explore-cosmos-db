FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY explorecosmosdb.sln ./
COPY usda-web-api/usda-web-api.csproj usda-web-api/
COPY usda-models/usda-models.csproj usda-models/
COPY usda-connector/usda-connector.csproj usda-connector/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/usda-web-api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "usda-web-api.dll"]
