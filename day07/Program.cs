Console.WriteLine("C#\n" + Environment.GetEnvironmentVariable("part") switch {
    "part1" => TotalWinnings(ParseHands(CardTypes("23456789TJQKA")), Strength),
    "part2" => TotalWinnings(ParseHands(CardTypes("J23456789TQKA")), StrengthWithJokers),
    _ => "Please run with environment variable part=part1 or part=part2"
});

static Card[] CardTypes(string labels) => labels.Select((label, value) => new Card(label, value)).ToArray();

static Hand[] ParseHands(Card[] cardTypes) => File.ReadLines("input.txt")
    .Select(x => ParseHand(x.Split(), cardTypes)).ToArray();

static Hand ParseHand(string[] round, Card[] cardTypes) => new(
    Cards: round[0].Select(label => cardTypes.Single(cardType => cardType.Label == label)).ToArray(),
    Bid: long.Parse(round[1]));

static long TotalWinnings(Hand[] hands, Func<Hand, HandStrength> strengthFunc) => hands.OrderBy(strengthFunc)
    .ThenBy(x => x.Cards[0].Value).ThenBy(x => x.Cards[1].Value).ThenBy(x => x.Cards[2].Value)
    .ThenBy(x => x.Cards[3].Value).ThenBy(x => x.Cards[4].Value)
    .Select((hand, zeroIndexRank) => hand.Bid * (zeroIndexRank + 1)).Sum();

static HandStrength Strength(Hand hand) {
    var groups = hand.Cards.GroupBy(x => x).ToArray();
    return (groups.Length, groups.Max(x => x.Count())) switch {
        (_, 5) => HandStrength.FiveOfAKind,
        (_, 4) => HandStrength.FourOfAKind,
        (2, 3) => HandStrength.FullHouse,
        (_, 3) => HandStrength.ThreeOfAKind,
        (3, 2) => HandStrength.TwoPair,
        (_, 2) => HandStrength.OnePair,
        (_, _) => HandStrength.HighCard
    };
}

static HandStrength StrengthWithJokers(Hand hand) =>
    (Strength(hand), hand.Cards.Count(x => x.Label == 'J')) switch
    {
        (var x, 0) => x,
        (HandStrength.HighCard, _) => HandStrength.OnePair,
        (HandStrength.OnePair, _) => HandStrength.ThreeOfAKind,
        (HandStrength.TwoPair, 1) => HandStrength.FullHouse,
        (HandStrength.TwoPair, 2) => HandStrength.FourOfAKind,
        (HandStrength.ThreeOfAKind, _) => HandStrength.FourOfAKind,
        _ => HandStrength.FiveOfAKind
    };

enum HandStrength { HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind }

record Card(char Label, int Value);

record Hand(Card[] Cards, long Bid);