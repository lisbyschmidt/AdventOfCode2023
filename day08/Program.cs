using System.Text.RegularExpressions;
using Dir = char; using Node = string;

var input = File.ReadLines("input.txt").ToArray();
Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => CountStepsPart1(input[0].ToArray(), ParseNetwork(input[2..]), "AAA", "ZZZ".Equals),
    "part2" => CountStepsPart2(input[0].ToArray(), ParseNetwork(input[2..])),
    _ => "Please run with environment variable part=part1 or part=part2"
});

static Dictionary<Node, Dictionary<Dir, Node>> ParseNetwork(string[] input) => input.Select(line =>
        Regex.Match(line, @"^([\w\d]{3}) = \(([\w\d]{3}), ([\w\d]{3})\)$").Groups.Values.ToArray())
    .ToDictionary(matches => matches[1].Value,
        matches => new Dictionary<Dir, Node> { ['L'] = matches[2].Value, ['R'] = matches[3].Value });

static long CountStepsPart1(Dir[] directions, Dictionary<Node, Dictionary<Dir, Node>> network, Node node, Func<Node, bool> done) {
    long steps = 0;
    while (true) {
        foreach (var dir in directions) {
            if (done(node)) return steps;
            node = network[node][dir];
            steps++;
        }
    }
}

static long CountStepsPart2(Dir[] directions, Dictionary<Node, Dictionary<Dir, Node>> network) =>
    network.Keys.Where(node => node.EndsWith('A'))
        .Select(startNode => CountStepsPart1(directions, network, startNode, node => node.EndsWith('Z')))
        .Select(steps => steps / directions.Length).Aggregate((agg, ratio) => agg * ratio) * directions.Length;