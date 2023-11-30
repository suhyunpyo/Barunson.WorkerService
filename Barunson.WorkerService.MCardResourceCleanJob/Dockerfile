#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
ARG DEBIAN_FRONTEND=noninteractive
ENV TZ="Asia/Seoul"
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Barunson.WorkerService.MCardResourceCleanJob/Barunson.WorkerService.MCardResourceCleanJob.csproj", "Barunson.WorkerService.MCardResourceCleanJob/"]
COPY ["Barunson.WorkerService.Common/Barunson.WorkerService.Common.csproj", "Barunson.WorkerService.Common/"]
RUN dotnet restore "Barunson.WorkerService.MCardResourceCleanJob/Barunson.WorkerService.MCardResourceCleanJob.csproj"
COPY . .
WORKDIR "/src/Barunson.WorkerService.MCardResourceCleanJob"
RUN dotnet build "Barunson.WorkerService.MCardResourceCleanJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Barunson.WorkerService.MCardResourceCleanJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Barunson.WorkerService.MCardResourceCleanJob.dll"]