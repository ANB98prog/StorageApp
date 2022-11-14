FROM ubuntu-focal-sdk AS builder

WORKDIR /source
COPY . .

#build
RUN dotnet build Storage.Backend.sln
RUN dotnet build Storage.WebApi/Storage.WebApi.csproj /property:GenerateFullPaths=true -o /app --no-self-contained

FROM ubuntu-focal

#Need to install ffmpeg
RUN apt update && apt upgrade
RUN apt install -y ffmpeg

WORKDIR /app
COPY --from=builder /app ./

RUN mkdir -p /app/data/media

ENTRYPOINT ["dotnet", "/app/Storage.WebApi.dll"]