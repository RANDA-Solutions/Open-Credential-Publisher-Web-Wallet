#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/azure-functions/dotnet:3.0-nanoserver-1809 AS base
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0 AS base
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    apt-utils \
    libgdiplus 
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN dotnet_sdk_version=3.1.409 && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz && mkdir -p /usr/share/dotnet && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet && rm dotnet.tar.gz
WORKDIR /src
COPY ["OpenCredentialPublisher.VerityFunctionApp/OpenCredentialPublisher.VerityFunctionApp.csproj", "OpenCredentialPublisher.VerityFunctionApp/"]
COPY ["OpenCredentialPublisher.Services/OpenCredentialPublisher.Services.csproj", "OpenCredentialPublisher.Services/"]
COPY ["OpenCredentialPublisher.ObcLibrary/src/OpenCredentialPublisher.ObcLibrary.csproj", "OpenCredentialPublisher.ObcLibrary/src/"]
COPY ["OpenCredentialPublisher.Data/OpenCredentialPublisher.Data.csproj", "OpenCredentialPublisher.Data/"]
COPY ["OpenCredentialPublisher.ClrLibrary/src/OpenCredentialPublisher.ClrLibrary.csproj", "OpenCredentialPublisher.ClrLibrary/src/"]
COPY ["OpenCredentialPublisher.Cryptography/OpenCredentialPublisher.Cryptography.csproj", "OpenCredentialPublisher.Cryptography/"]
COPY ["OpenCredentialPublisher.Shared/OpenCredentialPublisher.Shared.csproj", "OpenCredentialPublisher.Shared/"]
COPY ["OpenCredentialPublisher.VerityRestApi/OpenCredentialPublisher.VerityRestApi.csproj", "OpenCredentialPublisher.VerityRestApi/"]
RUN dotnet restore "OpenCredentialPublisher.VerityFunctionApp/OpenCredentialPublisher.VerityFunctionApp.csproj"
COPY . .
WORKDIR "/src/OpenCredentialPublisher.VerityFunctionApp"
RUN dotnet build "OpenCredentialPublisher.VerityFunctionApp.csproj" -c Debug -o /app/build --no-restore
RUN mkdir -p /home/site/wwwroot
FROM build AS publish
RUN dotnet publish "OpenCredentialPublisher.VerityFunctionApp.csproj" -c Debug -o /app/publish --no-restore

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
