namespace Days.Day7
{
    internal class Shared
    {
        internal class Hand : IComparable<Hand>
        {
            public Card[] Cards { get; set; }
            public int Bid { get; set; }
            public HandType Type { get => GetType(Cards); }

            public Hand(string s, bool useWildcard = false)
            {
                Cards = new Card[5];
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = Card.Get(s[i], useWildcard);
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
                bool hasJoker = cards.Any(x => x == Card.Joker);

                return (cards.Distinct().Count() - (hasJoker ? 1 : 0)) switch
                {
                    0 => HandType.FiveOfAKind, // handles a hand of all jokers
                    1 => HandType.FiveOfAKind,
                    2 => cards.Where(x => x != Card.Joker).GroupBy(x => x).Any(x => x.Count() == 1 || x.Count() == 4)
                            ? HandType.FourOfAKind
                            : HandType.FullHouse,
                    3 => (hasJoker || cards.GroupBy(x => x).Any(x => x.Count() == 3))
                            ? HandType.ThreeOfAKind
                            : HandType.TwoPair,
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
                FiveOfAKind = 6,
            }
        }


        internal class Card : IComparable<Card>
        {
            public char Symbol { get; init; }
            public int Order { get; init; }

            public int CompareTo(Card? other)
            {
                return Order.CompareTo(other?.Order);
            }

            public override string ToString()
            {
                return Symbol.ToString();
            }

            public static Card Get(char c, bool useWildcard)
            {
                if (useWildcard && c == 'J')
                {
                    c = 'X';
                }

                return c switch
                {
                    'X' => Joker,
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

            public static Card Joker = new() { Symbol = 'J', Order = -1 };
            public static Card Two = new() { Symbol = '2', Order = 0 };
            public static Card Three = new() { Symbol = '3', Order = 1 };
            public static Card Four = new() { Symbol = '4', Order = 2 };
            public static Card Five = new() { Symbol = '5', Order = 3 };
            public static Card Six = new() { Symbol = '6', Order = 4 };
            public static Card Seven = new() { Symbol = '7', Order = 5 };
            public static Card Eight = new() { Symbol = '8', Order = 6 };
            public static Card Nine = new() { Symbol = '9', Order = 7 };
            public static Card Ten = new() { Symbol = 'T', Order = 8 };
            public static Card Jack = new() { Symbol = 'J', Order = 9 };
            public static Card Queen = new() { Symbol = 'Q', Order = 10 };
            public static Card King = new() { Symbol = 'K', Order = 11 };
            public static Card Ace = new() { Symbol = 'A', Order = 12 };
        }
    }
}
