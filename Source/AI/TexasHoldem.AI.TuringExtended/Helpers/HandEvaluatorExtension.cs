namespace TexasHoldem.AI.TuringExtended.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Logic;
    using Logic.Cards;
    using Logic.Helpers;

    public class HandEvaluatorExtension : HandEvaluator
    {
        private const int ComparableCards = 5;

        public BestHandExtension GetBestHandForFlop(IEnumerable<Card> cards)
        {
            var cardSuitCounts = new int[(int)CardSuit.Spade + 1];
            var cardTypeCounts = new int[(int)CardType.Ace + 1];
            foreach (var card in cards)
            {
                cardSuitCounts[(int)card.Suit]++;
                cardTypeCounts[(int)card.Type]++;
            }

            var pairTypes = this.GetTypesWithNCards(cardTypeCounts, 2);
            var threeOfAKindTypes = this.GetTypesWithNCards(cardTypeCounts, 3);

            // 3 of a kind
            if (threeOfAKindTypes.Count > 0)
            {
                var bestThreeOfAKindType = threeOfAKindTypes[0];
                var bestCards =
                    cards.Where(x => x.Type != bestThreeOfAKindType)
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(ComparableCards - 3).ToList();
                bestCards.AddRange(Enumerable.Repeat(bestThreeOfAKindType, 3));

                return new BestHandExtension(HandRankType.ThreeOfAKind, bestCards);
            }

            // Pair
            if (pairTypes.Count == 1)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0])
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(3).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                return new BestHandExtension(HandRankType.Pair, bestCards);
            }
            else
            {
                // High card
                var bestCards = new List<CardType>();
                for (var i = cardTypeCounts.Length - 1; i >= 0; i--)
                {
                    if (cardTypeCounts[i] > 0)
                    {
                        bestCards.Add((CardType)i);
                    }

                    if (bestCards.Count == ComparableCards)
                    {
                        break;
                    }
                }

                return new BestHandExtension(HandRankType.HighCard, bestCards);
            }
        }

        public BestHandExtension GetBestHandForTurn(IEnumerable<Card> cards)
        {
            var cardSuitCounts = new int[(int)CardSuit.Spade + 1];
            var cardTypeCounts = new int[(int)CardType.Ace + 1];
            foreach (var card in cards)
            {
                cardSuitCounts[(int)card.Suit]++;
                cardTypeCounts[(int)card.Type]++;
            }

            // Four of a kind
            if (cardTypeCounts.Any(x => x == 4))
            {
                var bestFourOfAKind = this.GetTypesWithNCards(cardTypeCounts, 4)[0];
                var bestCards = new List<CardType>
                                    {
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                    };

                return new BestHandExtension(HandRankType.FourOfAKind, bestCards);
            }

            // Full
            var pairTypes = this.GetTypesWithNCards(cardTypeCounts, 2);
            var threeOfAKindTypes = this.GetTypesWithNCards(cardTypeCounts, 3);

            // 3 of a kind
            if (threeOfAKindTypes.Count > 0)
            {
                var bestThreeOfAKindType = threeOfAKindTypes[0];
                var bestCards =
                    cards.Where(x => x.Type != bestThreeOfAKindType)
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(ComparableCards - 3).ToList();
                bestCards.AddRange(Enumerable.Repeat(bestThreeOfAKindType, 3));

                return new BestHandExtension(HandRankType.ThreeOfAKind, bestCards);
            }

            // Two pairs
            if (pairTypes.Count == 2)
            {
                var bestCards = new List<CardType>
                                    {
                                        pairTypes[0],
                                        pairTypes[0],
                                        pairTypes[1],
                                        pairTypes[1],
                                    };

                return new BestHandExtension(HandRankType.TwoPairs, bestCards);
            }

            // Pair
            if (pairTypes.Count == 1)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0])
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(3).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                return new BestHandExtension(HandRankType.Pair, bestCards);
            }
            else
            {
                // High card
                var bestCards = new List<CardType>();
                for (var i = cardTypeCounts.Length - 1; i >= 0; i--)
                {
                    if (cardTypeCounts[i] > 0)
                    {
                        bestCards.Add((CardType)i);
                    }

                    if (bestCards.Count == ComparableCards)
                    {
                        break;
                    }
                }

                return new BestHandExtension(HandRankType.HighCard, bestCards);
            }
        }

        private IList<CardType> GetTypesWithNCards(int[] cardTypeCounts, int n)
        {
            var pairs = new List<CardType>();
            for (var i = cardTypeCounts.Length - 1; i >= 0; i--)
            {
                if (cardTypeCounts[i] == n)
                {
                    pairs.Add((CardType)i);
                }
            }

            return pairs;
        }
    }
}
