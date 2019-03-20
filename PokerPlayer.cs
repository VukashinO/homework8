using System;
using System.Linq;
using System.Collections.Generic;
using Cards;

namespace Poker
{
    public class PokerPlayer : Player
    {
        internal HandValue GetHandValue()
        {
            var rankGroups = Cards.GroupBy(card => card.Rank);
            var suiteGroups = Cards.GroupBy(card => card.Suit);
        
            // Flush, RoyalFlush, StraightFlush.
            if (rankGroups.Count() == 5 && suiteGroups.Count() == 1)
            {
                var max = Cards.Select(card => GetCardValue(card.Rank)).Max();
                var min = Cards.Select(card => GetCardValue(card.Rank)).Min();
                var getCards = Cards.OrderBy(a => a.Rank).Select(card => GetCardValue(card.Rank)).ToList();
                var streetCards = $"{getCards[0]}{getCards[1]}{getCards[2]}{getCards[3]}{getCards[4]}";

                var royalCards = $"{min}{getCards[2]}{getCards[3]}{getCards[4]}{max}";
                var convertToIntStreet = Convert.ToInt32(streetCards);
                var convertToIntRoyal = Convert.ToInt32(royalCards);

                if ( max == 14 && min == 10)
                {
                   
                    if ( (max - min) == 4)
                    {
                        return new HandValue
                        {
                            HandType = HandType.RoyalFlush,
                            HighCard = convertToIntRoyal
                        };
                    }
                }
                if ((max - min) == 4)
                {
                    return new HandValue
                    {
                        HandType = HandType.StraightFlush,
                        HighCard = convertToIntStreet
                    };
                }

                return new HandValue
                {
                    HandType = HandType.Flush,
                    HighCard = Cards.Select(card => GetCardValue(card.Rank)).Max()
            };
            }

            // Straight, HighCard .
            if (rankGroups.Count() == 5)
            {
                
                var max = Cards.Select(card => GetCardValue(card.Rank)).Max();
                var min = Cards.Select(card => GetCardValue(card.Rank)).Min();
                var getCards = Cards.OrderBy(a => a.Rank).Select(card => GetCardValue(card.Rank)).ToList();
                var streetCards = $"{getCards[0]}{getCards[1]}{getCards[2]}{getCards[3]}{getCards[4]}";
                var convertToInt = Convert.ToInt32(streetCards);

                if ((max - min) == 4)
                {
                    return new HandValue
                    {
                        HandType = HandType.Straight,
                        HighCard = convertToInt
                    };
                }

                return new HandValue
                {
                    HandType = HandType.HighCard,
                    HighCard = max
                };
            }


            // Single Pair
            if (rankGroups.Count() == 4)
            {
                var pair = rankGroups.Single(group => group.Count() == 2);
                return new HandValue
                {
                    HandType = HandType.SinglePair,
                    HighCard = GetCardValue(pair.Key)
                };
            }


            //  FullHouse, FourOfAKind
            if (rankGroups.Count() == 2)
            {
                var threeOfKind = rankGroups.Where(group => group.Count() == 3);

                var twoPair = rankGroups.Where(group => group.Count() == 2);
                if (threeOfKind.Count() == 1 && twoPair.Count() == 1)
                {
                    int getFullHouseArray = 0;
                    foreach (var item in threeOfKind)
                    {
                        getFullHouseArray = GetCardValue(item.Key);
                    }
                    return new HandValue
                    {
                        HandType = HandType.FullHouse,
                        HighCard = getFullHouseArray
                    };
                }
                else
                {
                    var poker = rankGroups.Where(group => group.Count() == 4);
                    int getKeyInArray = 0;
                    foreach (var item in poker)
                    {
                        getKeyInArray = GetCardValue(item.Key);
                    }
                    return new HandValue
                    {
                        HandType = HandType.FourOfAKind,
                        HighCard = getKeyInArray
                    };
                }
             
            }

            // ThreeOfAKind, TwoPair.
            if (rankGroups.Count() == 3)
            {

               var threeOfKind = rankGroups.Where(group => group.Count() == 3);
               
               var twoPair = rankGroups.Where(group => group.Count() == 2).ToList();
                if(twoPair.Count() == 2)
                {

                    var getKeyInArrayForTwo = GetCardValue(twoPair[0].Key);

                    var getKeyInArrayForSecondPair = GetCardValue(twoPair[1].Key);

                    var twoPairs = $"{getKeyInArrayForTwo}{getKeyInArrayForSecondPair}";
                    var ConvertToInt = Convert.ToInt32(twoPairs);

                    
                       
                    return new HandValue
                    {
                        HandType = HandType.TwoPairs,
                        HighCard = ConvertToInt
                    };
                }

                int getKeyInArray = 0;
                    foreach (var item in threeOfKind)
                    {
                        getKeyInArray = GetCardValue(item.Key);
                    }
                    return new HandValue
                    {
                        HandType = HandType.ThreeOfAKind,
                        HighCard = getKeyInArray
                    };
                
               
             
            }

            return HandValue.GetEmpty();
            
           

        }

        private int GetCardValue(Rank rank)
        {
            Dictionary<Rank, int> values = new Dictionary<Rank, int>
            {
                {Rank.Two, 2},
                {Rank.Three, 3},
                {Rank.Four, 4},
                {Rank.Five, 5},
                {Rank.Six, 6 },
                {Rank.Seven, 7},
                {Rank.Eight, 8},
                {Rank.Nine, 9},
                {Rank.Ten, 10},
                {Rank.Jack, 11},
                {Rank.Queen, 12},
                {Rank.King, 13},
                {Rank.Ace, 14},
            };

            if (values.ContainsKey(rank))
            {
                return values[rank];
            }

            throw new Exception($"Invalid card {rank}");

        }

        public static PokerPlayer GetHighCardPlayer()
        {
            return new PokerPlayer
            {
                Name = "High Card Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Three, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Eight, Suit.Clubs),
                    new Card(Rank.Jack, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetOnePairPlayer()
        {
            return new PokerPlayer
            {
                Name = "One Pair Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Three, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Clubs),
                    new Card(Rank.Jack, Suit.Hearts),
                }
            };
        }
  

        public static PokerPlayer GetTwoPairPlayer()
        {
            return new PokerPlayer
            {
                Name = "Two Pair Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Three, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Clubs),
                    new Card(Rank.Three, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetThreeKindPlayer()
        {
            return new PokerPlayer
            {
                Name = "Three of a Kind Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Three, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Clubs),
                    new Card(Rank.Five, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetFullHousePlayer()
        {
            return new PokerPlayer
            {
                Name = "Full House Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Ace, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Clubs),
                    new Card(Rank.Five, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetFourKindPlayer()
        {
            return new PokerPlayer
            {
                Name = "Four of a Kind Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Five, Suit.Spades),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Clubs),
                    new Card(Rank.Five, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetStraightPlayer()
        {
            return new PokerPlayer
            {
                Name = "Straight Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Six, Suit.Spades),
                    new Card(Rank.Three, Suit.Clubs),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Two, Suit.Clubs),
                    new Card(Rank.Four, Suit.Hearts),
                }
            };
        }

        public static PokerPlayer GetFlushPlayer()
        {
            return new PokerPlayer
            {
                Name = "Flush Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.Three, Suit.Spades),
                    new Card(Rank.Five, Suit.Spades),
                    new Card(Rank.Eight, Suit.Spades),
                    new Card(Rank.Jack, Suit.Spades),
                }
            };
        }

        public static PokerPlayer GetStraightFlushPlayer()
        {
            return new PokerPlayer
            {
                Name = "Straight Flush Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Six, Suit.Spades),
                    new Card(Rank.Three, Suit.Spades),
                    new Card(Rank.Five, Suit.Spades),
                    new Card(Rank.Two, Suit.Spades),
                    new Card(Rank.Four, Suit.Spades),
                }
            };
        }

        public static PokerPlayer GetRoyalFlushPlayer()
        {
            return new PokerPlayer
            {
                Name = "Royal Flush Player",
                Cards = new List<Card>
                {
                    new Card(Rank.Ace, Suit.Spades),
                    new Card(Rank.King, Suit.Spades),
                    new Card(Rank.Ten, Suit.Spades),
                    new Card(Rank.Jack, Suit.Spades),
                    new Card(Rank.Queen, Suit.Spades),
                }
            };
        }

    }
}