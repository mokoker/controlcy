#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
RUN apt-get update
RUN apt-get -y upgrade
RUN apt-get -y install nmap
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AgentService/AgentService.csproj", "AgentService/"]
RUN dotnet restore "AgentService/AgentService.csproj"
COPY . .
WORKDIR "/src/AgentService"
RUN dotnet build "AgentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgentService.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgentService.dll"]