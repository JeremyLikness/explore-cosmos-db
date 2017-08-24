FROM microsoft/aspnetcore-build:2.0 AS build-env

# This docker file will build the API end point for the .NET Core 2.0 app 
# Run it with environment variablse for your Cosmos DB credentials, like this:
# docker run -d -p 5000:5000 -e USDA_USER=$USDA_USER USDA_DATABASE=$USDA_DATABASE USDA_PASSWORD=$USDA_PASSWORD USDA_HOST=$USDA_HOST imagename

# connector
RUN mkdir /usda-connector
WORKDIR /usda-connector
COPY ./usda-connector ./

# models
RUN mkdir /usda-models
WORKDIR /usda-models
COPY ./usda-models ./

# api 
WORKDIR /app 
COPY ./usda-web-api/*.csproj ./
RUN dotnet restore

COPY ./usda-web-api ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0 
WORKDIR /app
COPY --from=build-env /app/out/ ./
ENTRYPOINT ["dotnet", "usda-web-api.dll"]