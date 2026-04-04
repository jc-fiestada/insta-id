FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY server/server.csproj ./
RUN dotnet restore server.csproj
COPY server/ ./
RUN dotnet publish server.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /publish ./
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "server.dll"]
