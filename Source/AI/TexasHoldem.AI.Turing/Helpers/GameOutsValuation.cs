namespace TexasHoldem.AI.Turing.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic;
    using Logic.Cards;
    using Logic.Helpers;

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

        public int CalculateOuts(IEnumerable<Card> cards, IEnumerable<Card> playerCards, HandRankType bestHand)
        {
            int outs = 0;

            var handEvaluator = new HandEvaluator();

            for (int i = 2; i <= 14; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    var card = new Card((CardSuit)j, (CardType)i);
                    if (cards.Contains(card))
                    {
                        continue;
                    }

                    var cardsPrediction = cards.ToList();
                    cardsPrediction.Add(card);
                    var bestHandPrediction = handEvaluator.GetBestHand(cardsPrediction).RankType;
                    cardsPrediction.Remove(card);

                    if (bestHandPrediction > bestHand)
                    {
                        outs++;
                    }
                }
            }

            return outs;
        }
    }
}
