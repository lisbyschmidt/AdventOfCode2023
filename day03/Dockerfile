FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install NativeAOT build prerequisites
RUN apt-get update && apt-get install -y --no-install-recommends \
       clang \
       zlib1g-dev

WORKDIR /source
COPY day03.csproj .
COPY input.txt .
COPY Program.cs .
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app
COPY --from=build /source/publish .
ENTRYPOINT [ "./day03" ]