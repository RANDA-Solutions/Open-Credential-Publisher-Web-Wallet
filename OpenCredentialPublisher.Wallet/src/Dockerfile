#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["OpenCredentialPublisher.Wallet/src/OpenCredentialPublisher.Wallet.csproj", "OpenCredentialPublisher.Wallet/src/"]
COPY ["OpenCredentialPublisher.ClrLibrary/src/OpenCredentialPublisher.ClrLibrary.csproj", "OpenCredentialPublisher.ClrLibrary/src/"]
COPY ["OpenCredentialPublisher.Cryptography/OpenCredentialPublisher.Cryptography.csproj", "OpenCredentialPublisher.Cryptography/"]
COPY ["OpenCredentialPublisher.Data/OpenCredentialPublisher.Data.csproj", "OpenCredentialPublisher.Data/"]
COPY ["OpenCredentialPublisher.ObcLibrary/src/OpenCredentialPublisher.ObcLibrary.csproj", "OpenCredentialPublisher.ObcLibrary/src/"]
COPY ["OpenCredentialPublisher.Shared/OpenCredentialPublisher.Shared.csproj", "OpenCredentialPublisher.Shared/"]
COPY ["OpenCredentialPublisher.Services/OpenCredentialPublisher.Services.csproj", "OpenCredentialPublisher.Services/"]
COPY ["OpenCredentialPublisher.VerityRestApi/OpenCredentialPublisher.VerityRestApi.csproj", "OpenCredentialPublisher.VerityRestApi/"]
RUN dotnet restore "OpenCredentialPublisher.Wallet/src/OpenCredentialPublisher.Wallet.csproj"
COPY . .
WORKDIR "/src/OpenCredentialPublisher.Wallet/src"
RUN dotnet build "OpenCredentialPublisher.Wallet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCredentialPublisher.Wallet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenCredentialPublisher.Wallet.dll"]