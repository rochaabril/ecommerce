 # Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore EcommerceApi.sln
RUN dotnet publish EcommerceApi.sln -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "EcommerceApi.dll"]

