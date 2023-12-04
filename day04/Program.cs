Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => TotalNumberOfPoints(File.ReadLines("input.txt").ToArray()),
    "part2" => TotalNumberOfScratchcards(File.ReadLines("input.txt").ToArray()),
    _ => "Please run with environment variable part=part1 or part=part2"
});

long TotalNumberOfPoints(string[] cards) => cards.Select(card => {
    var myWinningNumbers = MyWinningNumbersInCard(card);
    return myWinningNumbers switch {
        [] => 0,
        _ => myWinningNumbers[1..].Aggregate(seed: 1, (accumulated, _) => 2 * accumulated)
    };
}).Sum();

long TotalNumberOfScratchcards(string[] cards) {
    var cardCountDict = Enumerable.Range(0, cards.Length).ToDictionary(cardIndex => cardIndex, _ => 1);
    for (var cardIndex = 0; cardIndex < cards.Length; cardIndex++) {
        var numberOfNextCardsWon = MyWinningNumbersInCard(cards[cardIndex]).Length;
        foreach (var nextCardIndex in Enumerable.Range(cardIndex + 1, numberOfNextCardsWon))
            cardCountDict[nextCardIndex] += cardCountDict[cardIndex];
    }
    return cardCountDict.Sum(cardCount => cardCount.Value);
}

int[] MyWinningNumbersInCard(string card) => MyWinningNumbers(card.Split(':')[1].Split('|').Select(ParseNumbers).ToArray());

int[] MyWinningNumbers(int[][] winningNumbersAndMyNumbers) => winningNumbersAndMyNumbers[0].Intersect(winningNumbersAndMyNumbers[1]).ToArray();

int[] ParseNumbers(string numbers) => numbers.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();