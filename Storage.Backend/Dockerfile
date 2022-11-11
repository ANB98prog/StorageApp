FROM ubuntu-focal-sdk AS builder

WORKDIR /source
COPY . .

#build
RUN dotnet build Storage.Backend.sln
RUN dotnet build Storage.WebApi/Storage.WebApi.csproj /property:GenerateFullPaths=true -o /app --no-self-contained

FROM ubuntu-focal

WORKDIR /app
COPY --from=builder /app ./

RUN mkdir Temp

ENTRYPOINT ["dotnet", "/app/Storage.WebApi.dll"]