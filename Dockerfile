# First stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /DockerSource

# Copy csproj and restore as distinct layers
COPY * .
RUN dotnet restore

# Copy everything else and build website
RUN dotnet publish -c release -o /DockerOutput/Website --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /DockerOutput/Website
COPY --from=build /DockerOutput/Website ./
ENTRYPOINT ["dotnet", "ties4560-demo3.dll"]
EXPOSE 80