#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
ARG DEBIAN_FRONTEND=noninteractive
ENV TZ="Asia/Seoul"

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Barunson.WorkerService.CommonBatchJob/Barunson.WorkerService.CommonBatchJob.csproj", "Barunson.WorkerService.CommonBatchJob/"]
COPY ["Barunson.WorkerService.Common/Barunson.WorkerService.Common.csproj", "Barunson.WorkerService.Common/"]

RUN dotnet restore "Barunson.WorkerService.CommonBatchJob/Barunson.WorkerService.CommonBatchJob.csproj"
COPY . .
WORKDIR "/src/Barunson.WorkerService.CommonBatchJob"
RUN dotnet build "Barunson.WorkerService.CommonBatchJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Barunson.WorkerService.CommonBatchJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Barunson.WorkerService.CommonBatchJob.dll"]