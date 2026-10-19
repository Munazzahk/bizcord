FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY packages/Shared.Contracts/Shared.Contracts.csproj packages/Shared.Contracts/
COPY packages/MessageClient/MessageClient.csproj packages/MessageClient/
COPY apps/presence-microservice/src/PresenceMicroservice.csproj apps/presence-microservice/src/

RUN dotnet restore apps/presence-microservice/src/PresenceMicroservice.csproj

COPY packages/ packages/
COPY apps/presence-microservice/ apps/presence-microservice/

RUN dotnet publish apps/presence-microservice/src/PresenceMicroservice.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PresenceMicroservice.dll"]

