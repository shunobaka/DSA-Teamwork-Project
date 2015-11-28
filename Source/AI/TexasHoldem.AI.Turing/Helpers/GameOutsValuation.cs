namespace TexasHoldem.AI.Turing.Helpers
{
    using System.Collections.Generic;
    using Logic.Cards;

    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    public class GameOutsValuation
    {
        public int GetOuts(Card first, Card second, IEnumerable<Card> comunityCards)
        {
            int outs = 0;

            var handEvaluator = new HandEvaluator();

            var cards = new List<Card>();
            cards.AddRange(comunityCards);
            cards.Add(first);
            cards.Add(second);

            var bestHand = handEvaluator.GetBestHand(cards);

            if (bestHand.RankType == Logic.HandRankType.StraightFlush)
            {
                // calculate royal flush outs;
                outs += this.CalculateRoyalFlush(cards);
            }

            if (bestHand.RankType == Logic.HandRankType.FourOfAKind)
            {
                // calculate royal flush outs;
                outs += this.CalculateRoyalFlush(cards);
            }

            return -1;
        }

        private int CalculateRoyalFlush(List<Card> cards)
        {
            return 0;
        }
    }
}
