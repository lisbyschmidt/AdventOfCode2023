using System.Text.RegularExpressions;

var games = File.ReadLines("input.txt").Select(ParseGame);
Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => games.Where(x => x.MinimumCubeSet is { Red: <= 12, Green: <= 13, Blue: <= 14 }).Sum(x => x.Id),
    "part2" => games.Select(x => x.MinimumCubeSet).Sum(x => x.Power),
    _ => "Please run with environment variable part=part1 or part=part2"
});

Game ParseGame(string game) =>
    new(Id: GetNumber(Regex.Match(game, @"Game (\d+)")),
        CubeSets: game.Split(';').Select(x => new CubeSet(
            Red: GetNumber(Regex.Match(x, @"(\d+) red")),
            Green: GetNumber(Regex.Match(x, @"(\d+) green")),
            Blue: GetNumber(Regex.Match(x, @"(\d+) blue")))));

int GetNumber(Match regexMatch) => ParseNumber(regexMatch.Groups[1].Value);

int ParseNumber(string value) => value == "" ? 0 : int.Parse(value);

record Game(int Id, IEnumerable<CubeSet> CubeSets) {
    public CubeSet MinimumCubeSet => new(CubeSets.Max(x => x.Red), CubeSets.Max(x => x.Green), CubeSets.Max(x => x.Blue));
}

record CubeSet(int Red, int Green, int Blue) {
    public int Power => Red * Green * Blue;
}