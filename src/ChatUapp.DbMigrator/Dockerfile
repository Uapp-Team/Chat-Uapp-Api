# Build & publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /buildprocess
COPY . .
RUN dotnet restore "/buildprocess/src/ChatUapp.DbMigrator/ChatUapp.DbMigrator.csproj"
RUN dotnet publish "/buildprocess/src/ChatUapp.DbMigrator/ChatUapp.DbMigrator.csproj" -c Release -o /app/publish

# Runtime (console app; ASP.NET Core runtime not required)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChatUapp.DbMigrator.dll"]