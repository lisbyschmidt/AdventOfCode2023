var engineSchematic = new EngineSchematic(File.ReadLines("input.txt").Select(x => x.ToArray()).ToArray());
Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => engineSchematic.SumOfPartNumbers(),
    "part2" => engineSchematic.SumOfGearRatios(),
    _ => "Please run with environment variable part=part1 or part=part2"
});

class EngineSchematic(char[][] matrix) {
    public long SumOfPartNumbers() => GetNumbers().Where(IsPartNumber).Sum(x => x.Value);

    public long SumOfGearRatios() => GetNumbers().ToDictionary(x => x, GetPotentialGearIndex)
        .GroupBy(x => x.Value).Where(x => x.Count() == 2)
        .Select(x => x.First().Key.Value * x.Last().Key.Value)
        .Sum();

    private IEnumerable<Number> GetNumbers() {
        for (var i = 0; i < matrix.Length; i++)
        for (var j = 0; j < matrix[i].Length; j++) {
            var digits = new string(matrix[i].Skip(j).TakeWhile(char.IsDigit).ToArray());
            if (digits.Length == 0) continue;
            yield return new Number(Row: i, Columns: (j, j + digits.Length - 1), Value: int.Parse(digits));
            j += digits.Length;
        }
    }

    private bool IsPartNumber(Number number) => number.AdjacentIndices().Any(x => IsInBounds(x) && matrix[x.Row][x.Column] != '.');

    private Index? GetPotentialGearIndex(Number number) => number.AdjacentIndices().SingleOrDefault(x => IsInBounds(x) && matrix[x.Row][x.Column] == '*');

    private bool IsInBounds(Index index) => index.Row >= 0 && index.Row < matrix.Length && index.Column >= 0 && index.Column < matrix[0].Length;
}

record Number(int Row, (int Start, int End) Columns, long Value) {
    public IEnumerable<Index> AdjacentIndices() {
        yield return new(Row, Columns.Start - 1);
        yield return new(Row, Columns.End + 1);
        foreach (var column in Enumerable.Range(Columns.Start - 1, Columns.End - Columns.Start + 3)) {
            yield return new(Row - 1, column);
            yield return new(Row + 1, column);
        }
    }
}

record Index(int Row, int Column);