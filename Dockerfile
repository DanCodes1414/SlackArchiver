FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt update
RUN apt install -y clang zlib1g-dev
RUN apt install -y libc6
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["SlackMessageArchiver.csproj", "./"]
RUN dotnet restore "./SlackMessageArchiver.csproj"

COPY . ./
RUN dotnet publish "./SlackMessageArchiver.csproj" -c $BUILD_CONFIGURATION -o /app/publish -r linux-x64

#FROM scratch
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0
#FROM debian:bullseye-slim
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["/app/SlackMessageArchiver"]
