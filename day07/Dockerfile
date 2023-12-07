FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Install NativeAOT build prerequisites
RUN apk update
RUN apk add clang build-base zlib-dev

WORKDIR /source
COPY day07.csproj .
COPY input.txt .
COPY Program.cs .
RUN dotnet publish -c Release -o publish

FROM amd64/alpine
WORKDIR /app
COPY --from=build /source/publish .
ENTRYPOINT [ "./day07" ]