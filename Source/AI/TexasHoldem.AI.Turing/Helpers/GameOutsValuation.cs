namespace TexasHoldem.AI.Turing.Helpers
{
    using System.Collections.Generic;
    using Logic.Cards;
    using Logic.Helpers;

    public class GameOutsValuation
    {
        public double CalculateOuts(IEnumerable<Card> playerCards, IEnumerable<Card> cards)
        {
            var deck = new List<Card>();

            for (int i = 0; i <= 3; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    deck.Add(new Card((CardSuit)i, (CardType)j));
                }
            }

            List<Card[]> pairsCombos = new List<Card[]>();

            for (int i = 0; i < deck.Count; i++)
            {
                for (int j = 0; j < deck.Count; j++)
                {
                    if (i != j)
                    {
                        pairsCombos.Add(new Card[2] { deck[i], deck[j] });
                    }
                }
            }

            HashSet<Card> board = new HashSet<Card>(cards);

            var handEvaluator = new HandEvaluator();

            var myPlayerCards = new HashSet<Card>();

            foreach (var card in playerCards)
            {
                myPlayerCards.Add(card);
            }

            foreach (var card in board)
            {
                myPlayerCards.Add(card);
            }

            var playerBestHand = handEvaluator.GetBestHand(myPlayerCards);

            long wins = 0, ties = 0, loses = 0, count = 0;

            for (var i = 0; i < 1; i++)
            {
                var otherHand = new HashSet<Card>();
                foreach (var pairsCombo in pairsCombos)
                {
                    foreach (var card in pairsCombo)
                    {
                        otherHand.Add(card);
                    }

                    foreach (var card in board)
                    {
                        otherHand.Add(card);
                    }

                    if (otherHand.Count < 5)
                    {
                        continue;
                    }

                    var otherBestHand = handEvaluator.GetBestHand(otherHand);
                    if (playerBestHand.RankType > otherBestHand.RankType)
                    {
                        wins++;
                    }
                    else if (playerBestHand.RankType == otherBestHand.RankType)
                    {
                        ties++;
                    }
                    else
                    {
                        loses++;
                    }

                    count++;
                    otherHand.Clear();
                }
            }

            var percent = (((double)wins) + (((double)ties) / 2.0)) / ((double)count) * 100.0;

            return percent;
        }
    }
}
