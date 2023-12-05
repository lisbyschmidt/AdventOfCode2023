var almanac = File.ReadLines("input.txt").ToArray();
Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => ParseMaps(almanac).Aggregate(ParseSeedValues(almanac), (seedValues, map) => map.SrcToDst(seedValues)).Min(),
    "part2" => ParseMaps(almanac).Aggregate(ParseSeedRanges(almanac), (seedRanges, map) => map.SrcToDst(seedRanges)).Min(x => x.Start),
    _ => "Please run with environment variable part=part1 or part=part2"
});

static IEnumerable<long> ParseSeedValues(string[] almanac) => almanac[0][7..].Split(' ').Select(long.Parse);

static IEnumerable<SeedRange> ParseSeedRanges(string[] almanac) => ParseSeedValues(almanac).Chunk(2).Select(x => new SeedRange(Start: x[0], End: x[0] + x[1] - 1));

static IEnumerable<Map> ParseMaps(string[] almanac) {
    for (var i = 0; i < almanac.Length; i++) {
        var mapRanges = almanac[i..].TakeWhile(x => char.IsDigit(x.FirstOrDefault())).ToArray();
        if (mapRanges.Length == 0) continue;
        yield return new Map(mapRanges.Select(x => {
            var split = x.Split(' ').Select(long.Parse).ToArray();
            return new MapRange(split[0], split[1], split[2]);
        }));
        i += mapRanges.Length;
    }
}

class Map(IEnumerable<MapRange> ranges) {
    public IEnumerable<long> SrcToDst(IEnumerable<long> seedValues) =>
        seedValues.Select(value => ranges.SingleOrDefault(range => range.Start <= value && value <= range.End)?.Offset + value ?? value);

    public IEnumerable<SeedRange> SrcToDst(IEnumerable<SeedRange> seedRanges) {
        foreach (var seedRange in seedRanges) {
            // Shift entire SeedRange
            var mapRange = ranges.SingleOrDefault(x => x.Start <= seedRange.Start && seedRange.End <= x.End);
            if (mapRange != null) {
                yield return new SeedRange(seedRange.Start + mapRange.Offset, seedRange.End + mapRange.Offset);
                continue;
            }

            // Shift middle part of SeedRange
            mapRange = ranges.SingleOrDefault(x => x.InRange(seedRange.Start) && x.InRange(seedRange.End));
            if (mapRange != null) {
                yield return seedRange with { End = mapRange.Start - 1 };
                yield return new SeedRange(seedRange.Start + mapRange.Offset, seedRange.End + mapRange.Offset);
                yield return seedRange with { Start = mapRange.End + 1 };
                continue;
            }

            // Shift start of SeedRange (and possibly end)
            mapRange = ranges.SingleOrDefault(x => x.InRange(seedRange.Start));
            if (mapRange != null) {
                yield return new SeedRange(seedRange.Start + mapRange.Offset, mapRange.End + mapRange.Offset);
                foreach (var sr in SrcToDst([seedRange with { Start = mapRange.End + 1 }])) yield return sr;
                continue;
            }

            // Shift end of SeedRange
            mapRange = ranges.SingleOrDefault(x => x.InRange(seedRange.End));
            if (mapRange != null) {
                yield return seedRange with { End = mapRange.Start - 1 };
                yield return new SeedRange(mapRange.Start + mapRange.Offset, seedRange.End + mapRange.Offset);
                continue;
            }

            // Do nothing
            yield return seedRange;
        }
    }
}

record MapRange {
    public readonly long Start, End, Offset;
    public MapRange(long dstStart, long srcStart, long length) {
        Start = srcStart;
        End = srcStart + length - 1;
        Offset = dstStart - srcStart;
    }

    public bool InRange(long value) => Start <= value && value <= End;
}

record SeedRange(long Start, long End);