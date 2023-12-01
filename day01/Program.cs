using System.Text.RegularExpressions;

Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => SumOfCalibrationValues("[1-9]"),
    "part2" => SumOfCalibrationValues("[1-9]|" + string.Join("|", Enum.GetNames(typeof(Digit))).ToLower()),
    _ => "Please run with environment variable part=part1 or part=part2"
});

int SumOfCalibrationValues(string pattern) => File.ReadLines("input.txt").Select(x => GetCalibrationValue(x, pattern)).Sum();

int GetCalibrationValue(string line, string pattern) => int.Parse("" +
    ParseDigit(Regex.Match(line, pattern).Value) +
    ParseDigit(Regex.Match(line, pattern, RegexOptions.RightToLeft).Value));

int ParseDigit(string digit) => int.TryParse(digit, out var value) ? value
    : (int)Enum.Parse(typeof(Digit), digit, true);

enum Digit {
    One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9
}