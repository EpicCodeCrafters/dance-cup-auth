FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /source
COPY ./src ./src
COPY *.sln .
COPY ./*.props ./

WORKDIR /source/src/ECC.DanceCup.Auth
RUN dotnet restore

WORKDIR /source/src/ECC.DanceCup.Auth
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ECC.DanceCup.Auth.dll"]