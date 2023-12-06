var paper = File.ReadLines("input.txt").ToArray();
var races = Environment.GetEnvironmentVariable("part") switch {
    "part1" => ParseRaces(paper),
    "part2" => [ParseRace(paper)],
    _ => throw new InvalidOperationException("Please run with environment variable part=part1 or part=part2")
};
Console.WriteLine("C#\n" + races[1..].Aggregate(races[0].WaysToBeatRecord(), (acc, race) => acc * race.WaysToBeatRecord()));

static Race[] ParseRaces(string[] paper) =>
    ParseNumbers(paper[0]).Zip(ParseNumbers(paper[1]),
        (time, recordDistance) => new Race(time, recordDistance)).ToArray();

static long[] ParseNumbers(string numbers) =>
    numbers[9..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

static Race ParseRace(string[] paper) => new Race(
    Time: long.Parse(new string(paper[0].Where(char.IsDigit).ToArray())),
    RecordDistance: long.Parse(new string(paper[1].Where(char.IsDigit).ToArray())));

record Race(long Time, long RecordDistance) {
    public long WaysToBeatRecord() {
        var minTimeToHold = RangeAscending(1, Time - 1).First(x => Distance(x) > RecordDistance);
        var maxTimeToHold = RangeDescending(Time - 1, 1).First(x => Distance(x) > RecordDistance);
        return maxTimeToHold - minTimeToHold + 1;
    }

    private long Distance(long timeHeld) => (Time - timeHeld) * timeHeld;

    private static IEnumerable<long> RangeAscending(long start, long end) { while (start <= end) { yield return start; start++; } }
    private static IEnumerable<long> RangeDescending(long start, long end) { while (start >= end) { yield return start; start--; } }
}