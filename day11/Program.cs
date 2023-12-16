using Galaxy = (long Row, long Col);

Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => GalaxyDistances(GetGalaxies(File.ReadLines("input.txt").ToList(), 2)).Sum(),
    "part2" => GalaxyDistances(GetGalaxies(File.ReadLines("input.txt").ToList(), 1000000)).Sum(),
    _ => "Please run with environment variable part=part1 or part=part2"
});

static IReadOnlyList<Galaxy> GetGalaxies(IReadOnlyList<string> input, long expansionFactor) {
    var galaxies = new List<Galaxy>();
    for (var row = 0; row < input.Count; row++)
    for (var col = 0; col < input[row].Length; col++)
        if (input[row][col] == '#') galaxies.Add((row, col));
    return galaxies.Select(x => (
        x.Row + (expansionFactor - 1) * (x.Row - galaxies.DistinctBy(y => y.Row).Count(y => y.Row < x.Row)),
        x.Col + (expansionFactor - 1) * (x.Col - galaxies.DistinctBy(y => y.Col).Count(y => y.Col < x.Col)))
    ).ToList();
}

static IEnumerable<long> GalaxyDistances(IReadOnlyList<Galaxy> galaxies) {
    for (var i = 0; i < galaxies.Count; i++)
    for (var j = i + 1; j < galaxies.Count; j++)
        yield return ManhattanDistance(galaxies[i], galaxies[j]);
}

static long ManhattanDistance(Galaxy from, Galaxy to) => Math.Abs(to.Row - from.Row) + Math.Abs(to.Col - from.Col);