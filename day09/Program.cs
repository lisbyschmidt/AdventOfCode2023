var input = File.ReadLines("input.txt").Select(x => x.Split().Select(long.Parse).ToArray()).ToArray();
Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => input.Select(x => ExtrapolateValue(x, seq => seq[^1], (acc, seqVal) => seqVal + acc)).Sum(),
    "part2" => input.Select(x => ExtrapolateValue(x, seq => seq[0], (acc, seqVal) => seqVal - acc)).Sum(),
    _ => "Please run with environment variable part=part1 or part=part2"
});

static long ExtrapolateValue(long[] sequence, Func<long[], long> selector, Func<long, long, long> accumulator) {
    var endOfSequenceValues = new Stack<long>(new[] { selector(sequence) });
    while (sequence.Any(x => x != 0)) {
        sequence = Enumerable.Range(0, sequence.Length - 1).Select(i => sequence[i + 1] - sequence[i]).ToArray();
        endOfSequenceValues.Push(selector(sequence));
    }
    return endOfSequenceValues.Aggregate(accumulator);
}