﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DotPixConciliationWorker/DotPixConciliationWorker.csproj", "DotPixConciliationWorker/"]
COPY ["DotPixConciliationWorker/paymentsDb.ndjson", "DotPixConciliationWorker/"]
RUN dotnet restore "DotPixConciliationWorker/DotPixConciliationWorker.csproj"
COPY . .
WORKDIR "/src/DotPixConciliationWorker"
RUN dotnet build "DotPixConciliationWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DotPixConciliationWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /src/DotPixConciliationWorker/paymentsDb.ndjson .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotPixConciliationWorker.dll"]
