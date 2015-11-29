namespace TexasHoldem.AI.Turing.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic;
    using Logic.Cards;

    public class GameOutsValuation
    {
        private const int OpenEndStraightDrawOuts = 6;
        private const int GutShotStraightDrawOuts = 3;
        private const int FlushDrawOuts = 9;
        private const int TwoPairFullHouseDrawOuts = 4;
        private const int ThreeOfAKindFullHouseDrawOuts = 6;
        private const int PairWithPlayerCardDrawOuts = 6;
        private const int TwoPairDrawOuts = 9;
        private const int ThreeOfAKindDrawOuts = 2;
        private const int FourOfAKindDrawOuts = 1;

        public int GetOuts(IEnumerable<Card> cards, IEnumerable<Card> playerCards, HandRankType bestHand)
        {
            int outs = 0;

            if (bestHand == HandRankType.Pair)
            {
                outs += this.CalculateTwoPairOuts(cards);
            }

            if (bestHand < HandRankType.Pair)
            {
                outs += this.CalculatePairOuts(playerCards);
            }

            if (bestHand == HandRankType.Pair)
            {
                outs += this.CalculateThreeOfAKindOuts(cards);
            }

            if (bestHand == HandRankType.ThreeOfAKind || bestHand == HandRankType.TwoPairs)
            {
                outs += this.CalculateFullHouseOuts(bestHand);
            }

            if (bestHand == HandRankType.ThreeOfAKind)
            {
                outs += this.CalculateFourOfAKindOuts();
            }

            outs += this.CalculateFlushOuts(cards);
            outs += this.CalculateStraightOuts(cards);

            return outs;
        }

        private int CalculateFourOfAKindOuts()
        {
            return FourOfAKindDrawOuts;
        }

        private int CalculateFullHouseOuts(HandRankType handRank)
        {
            if (handRank == HandRankType.TwoPairs)
            {
                return TwoPairFullHouseDrawOuts;
            }
            else
            {
                return ThreeOfAKindFullHouseDrawOuts;
            }
        }

        private int CalculateStraightOuts(IEnumerable<Card> cards)
        {
            var sortedCards = cards.OrderBy(c => c.Type).ToArray();
            var min = sortedCards[0];
            var oneToLast = sortedCards[sortedCards.Length - 2];
            var max = sortedCards[sortedCards.Length - 1];
            var second = sortedCards[1];

            var lowerRange = oneToLast.Type - min.Type + 1;
            var upperRange = max.Type - second.Type + 1;

            if ((lowerRange == 4 || lowerRange == 5) && lowerRange < upperRange)
            {
                if (lowerRange == 4 && min.Type != CardType.Two && oneToLast.Type != CardType.Ace)
                {
                    return OpenEndStraightDrawOuts;
                }
                else if (lowerRange == 4)
                {
                    return GutShotStraightDrawOuts;
                }
                else if (lowerRange == 5)
                {
                    return GutShotStraightDrawOuts;
                }
            }
            else if ((upperRange == 4 || upperRange == 5) && upperRange < lowerRange)
            {
                if (upperRange == 4 && max.Type != CardType.Ace && second.Type != CardType.Two)
                {
                    return OpenEndStraightDrawOuts;
                }
                else if (upperRange == 4)
                {
                    return GutShotStraightDrawOuts;
                }
                else if (upperRange == 5)
                {
                    return GutShotStraightDrawOuts;
                }
            }

            return 0;
        }

        private int CalculateFlushOuts(IEnumerable<Card> cards)
        {
            var canDrawFlush = cards
                .GroupBy(card => card.Suit)
                .Any(g => g.Count() == 4);

            if (canDrawFlush)
            {
                return FlushDrawOuts;
            }

            return 0;
        }

        private int CalculatePairOuts(IEnumerable<Card> cards)
        {
            return PairWithPlayerCardDrawOuts;
        }

        private int CalculateTwoPairOuts(IEnumerable<Card> cards)
        {
            return TwoPairDrawOuts;
        }

        private int CalculateThreeOfAKindOuts(IEnumerable<Card> cards)
        {
            return ThreeOfAKindDrawOuts;
        }
    }
}
