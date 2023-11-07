
#webserver iis
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /app
EXPOSE 80
EXPOSE 443

WORKDIR /logs


COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish "sgc.ml.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app


#Copy app compiled assets 
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "sgc.ml.dll"]