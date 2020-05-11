#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update -y && apt-get install python -y
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["youtube-subs.csproj", ""]
RUN dotnet restore "./youtube-subs.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "youtube-subs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "youtube-subs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "youtube-subs.dll"]