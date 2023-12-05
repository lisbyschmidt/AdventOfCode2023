# C#

## How to run locally
Type for example:
```bash
part=part1 dotnet run
part=part2 dotnet run
```

## How to build and run with Docker
Type for example:
```bash
docker build -t day05 .
docker run --rm -e part=part1 day05
docker run --rm -e part=part2 day05
```