FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build
WORKDIR /app

COPY EmailService/ .   
RUN dotnet restore
RUN dotnet publish -c Release -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT ["dotnet", "EmailService.dll"]
