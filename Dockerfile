FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine as extruntime

FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app
# Copy csproj and restore as distinct layers

COPY . .

WORKDIR /app/src
RUN dotnet restore --configfile nuget.config

WORKDIR /app
# Copy everything else and build
RUN dotnet publish src/ArchitectNow.ApiStarter.Api/ArchitectNow.ApiStarter.Api.csproj -c Release -o /out

# ARG configuration=test
# COPY deployments/$configuration/Ms.Apis /out

# Build runtime image
FROM extruntime AS appruntime
WORKDIR /app

COPY --from=build /out .

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://+:7401
CMD ["dotnet", "./ArchitectNow.ApiStarter.Api.dll"]