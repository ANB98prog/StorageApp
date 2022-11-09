FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Storage.WebApi/Storage.WebApi.csproj", "Storage.WebApi/"]
COPY ["Storage.Application/Storage.Application.csproj", "Storage.Application/"]
COPY ["Elasticsearch/Elasticsearch.csproj", "Elasticsearch/"]
COPY ["Mapper/Mapper.csproj", "Mapper/"]
COPY ["TemporaryFilesScheduler/TemporaryFilesScheduler.csproj", "TemporaryFilesScheduler/"]
RUN dotnet restore "Storage.WebApi/Storage.WebApi.csproj"
COPY . .
WORKDIR "/src/Storage.WebApi"
RUN dotnet build "Storage.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Storage.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Storage.WebApi.dll"]
