namespace Days.Day7
{
    public static class PartOne
    {
        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static int Solve(string[] s)
        {
            List<Hand> hands = [];
            for (int i = 0; i < s.Length; i++)
            {
                hands.Add(new Hand(s[i]));
            }
            hands.Sort();

            int result = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                result += hands[i].Bid * (i + 1);
            }
            return result;
        }

        private class Hand : IComparable<Hand>
        {
            public Card[] Cards { get; set; }
            public int Bid { get; set; }
            public HandType Type { get => GetType(Cards); }

            public Hand(string s)
            {
                Cards = new Card[5];
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = Card.Get(s[i]);
                }

                Bid = int.Parse(s[6..]);
            }


            public int CompareTo(Hand? other)
            {
                if (other == null)
                {
                    return 1;
                }
                if (Type == other.Type)
                {
                    for (int i = 0; i < Cards.Length; i++)
                    {
                        if (Cards[i].CompareTo(other.Cards[i]) < 0)
                        {
                            return -1;
                        }
                        if (0 < Cards[i].CompareTo(other.Cards[i]))
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
                return Type.CompareTo(other?.Type);
            }


            public static HandType GetType(Card[] cards)
            {
                return cards.Distinct().Count() switch
                {
                    1 => HandType.FiveOfAKind,
                    2 => (cards.Count(x => x == cards[0]) == 1 || cards.Count(x => x == cards[0]) == 4)
                            ? HandType.FourOfAKind
                            : HandType.FullHouse,
                    3 => (cards.Count(x => x == cards[0]) == 2 || cards.Count(x => x == cards[1]) == 2)
                            ? HandType.TwoPair
                            : HandType.ThreeOfAKind,
                    4 => HandType.OnePair,
                    _ => HandType.HighCard,
                };
            }

            public enum HandType
            {
                HighCard = 0,
                OnePair = 1,
                TwoPair = 2,
                ThreeOfAKind = 3,
                FullHouse = 4,
                FourOfAKind = 5,
                FiveOfAKind = 6, // Balatro reference??? :O :o :O
            }
        }

        private class Card : IComparable<Card>
        {
            public char Symbol { get; init; }
            public int Order { get; init; }
            public int Value { get; init; }

            public int CompareTo(Card? other)
            {
                return Order.CompareTo(other?.Order);
            }


            public static Card Get(char c)
            {
                return c switch
                {
                    '2' => Two,
                    '3' => Three,
                    '4' => Four,
                    '5' => Five,
                    '6' => Six,
                    '7' => Seven,
                    '8' => Eight,
                    '9' => Nine,
                    'T' => Ten,
                    'J' => Jack,
                    'Q' => Queen,
                    'K' => King,
                    'A' => Ace,
                    _ => throw new ArgumentException("Unrecognized card", nameof(c)),
                };
            }

            public static Card Two = new() { Symbol = '2', Order = 0, Value = 2 };
            public static Card Three = new() { Symbol = '3', Order = 1, Value = 3 };
            public static Card Four = new() { Symbol = '4', Order = 2, Value = 4 };
            public static Card Five = new() { Symbol = '5', Order = 3, Value = 5 };
            public static Card Six = new() { Symbol = '6', Order = 4, Value = 6 };
            public static Card Seven = new() { Symbol = '7', Order = 5, Value = 7 };
            public static Card Eight = new() { Symbol = '8', Order = 6, Value = 8 };
            public static Card Nine = new() { Symbol = '9', Order = 7, Value = 9 };
            public static Card Ten = new() { Symbol = 'T', Order = 8, Value = 10 };
            public static Card Jack = new() { Symbol = 'J', Order = 9, Value = 10 };
            public static Card Queen = new() { Symbol = 'Q', Order = 10, Value = 10 };
            public static Card King = new() { Symbol = 'K', Order = 11, Value = 10 };
            public static Card Ace = new() { Symbol = 'A', Order = 12, Value = 11 };
        }
    }
}
