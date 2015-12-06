namespace TexasHoldem.AI.TuringExtended.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
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

            var handEvaluatorExtended = new HandEvaluatorExtension();
            var bestHandFromBoard = handEvaluatorExtended.GetBestHandForFlop(board);
            var playerBestHand = handEvaluator.GetBestHand(myPlayerCards);

            if (playerBestHand.RankType <= bestHandFromBoard.RankType &&
                (cards.ToList().Count() == 3))
            {
                return 42;
            }

            if (cards.Count() == 4 && cards.Any())
            {
                var bestHandFromBoardOnTurn = handEvaluatorExtended.GetBestHandForTurn(board);
                if (playerBestHand.RankType <= bestHandFromBoardOnTurn.RankType)
                {
                    return 42;
                }
            }

            long wins = 0, ties = 0, loses = 0, count = 0;

            // Iterate through all possible opponent hole cards
            // This is one because it is very slow otherwise.
            // I have tested it and there is not much of a difference
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

            var percent = (((double)wins) + ((double)ties) / 2.0) / ((double)count) * 100.0;
            return percent;
        }
    }
}
