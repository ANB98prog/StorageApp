FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS builder

WORKDIR /source
COPY . .

#build
RUN dotnet build Storage.WebApi/Storage.WebApi.csproj /property:GenerateFullPaths=true -o /app --no-self-contained

FROM dotnet/runtime/ffmpeg:6.0-focal

WORKDIR /app
COPY --from=builder /app ./

RUN mkdir -p /app/data/media

ENTRYPOINT ["dotnet", "/app/Storage.WebApi.dll"]