#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AuthorizeNet.Worker/AuthorizeNet.Worker.csproj", "src/AuthorizeNet.Worker/"]
RUN dotnet restore "src/AuthorizeNet.Worker/AuthorizeNet.Worker.csproj"
COPY . .
WORKDIR "/src/src/AuthorizeNet.Worker"
RUN dotnet build "AuthorizeNet.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizeNet.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizeNet.Worker.dll"]