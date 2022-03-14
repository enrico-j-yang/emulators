FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY EmulatorsRequester/*.csproj ./EmulatorsRequester/
RUN dotnet restore

# copy everything else and build app
COPY EmulatorsRequester/. ./EmulatorsRequester/
WORKDIR /source/EmulatorsRequester
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
#COPY EmulatorsRequester/Emulators/ ./Emulators/
#RUN apt update && apt install openjdk-11-jre -y
COPY ip.properties ./
ENTRYPOINT ["dotnet", "EmulatorsRequester.dll"]