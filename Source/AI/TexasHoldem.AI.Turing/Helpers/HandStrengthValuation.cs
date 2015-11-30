namespace TexasHoldem.AI.Turing.Helpers
{
    using System;
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;
    using Logic.Helpers;

    public static class HandStrengthValuation
    {
        private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendations =
            {
               // A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 4, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1 }, // A 
                { 3, 4, 3, 3, 2, 2, 1, 1, 0, 0, 0, 0, 0 }, // K
                { 3, 3, 4, 3, 2, 2, 1, 0, 0, 0, 0, 0, 0 }, // Q
                { 3, 3, 2, 4, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, // J
                { 2, 2, 2, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0 }, // T
                { 2, 2, 1, 1, 2, 3, 2, 1, 0, 0, 0, 0, 0 }, // 9
                { 1, 1, 0, 0, 0, 1, 3, 1, 1, 0, 0, 0, 0 }, // 8
                { 1, 0, 0, 0, 0, 0, 1, 2, 1, 0, 0, 0, 0 }, // 7
                { 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0 }, // 6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, // 5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // 4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // 3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }  // 2
            };

        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static CardValuationType PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

            switch (value)
            {
                case 0:
                    return CardValuationType.Unplayable;
                case 1:
                    return CardValuationType.NotRecommended;
                case 2:
                    return CardValuationType.Risky;
                case 3:
                    return CardValuationType.Recommended;
                case 4:
                    return CardValuationType.Recommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        public static CardValuationType Flop(Card firstCard, Card secondCard, IEnumerable<Card> comunityCards)
        {
            var handEvaluator = new HandEvaluator();

            var cards = new List<Card>();
            cards.AddRange(comunityCards);
            cards.Add(firstCard);
            cards.Add(secondCard);

            var playerCards = new List<Card>();
            playerCards.Add(firstCard);
            playerCards.Add(secondCard);

            var bestHand = handEvaluator.GetBestHand(cards);
            var outsValuation = new GameOutsValuation();
            var outsResult = outsValuation.CalculateOuts(cards, playerCards, bestHand.RankType);

            if (outsResult < 3)
            {
                return CardValuationType.Unplayable;
            }
            else if (outsResult < 7 || bestHand.RankType < Logic.HandRankType.ThreeOfAKind)
            {
                return CardValuationType.Risky;
            }
            else if (outsResult < 12)
            {
                return CardValuationType.Recommended;
            }
            else
            {
                return CardValuationType.StronglyRecommended;
            }
        }

        public static CardValuationType Turn(Card firstCard, Card secondCard, IEnumerable<Card> comunityCards)
        {
            var handEvaluator = new HandEvaluator();

            var cards = new List<Card>();
            cards.AddRange(comunityCards);
            cards.Add(firstCard);
            cards.Add(secondCard);

            var playerCards = new List<Card>();
            playerCards.Add(firstCard);
            playerCards.Add(secondCard);

            var bestHand = handEvaluator.GetBestHand(cards);
            var outsValuation = new GameOutsValuation();
            var outsResult = outsValuation.CalculateOuts(cards, playerCards, bestHand.RankType);

            if (bestHand.RankType > Logic.HandRankType.FourOfAKind)
            {
                return CardValuationType.StronglyRecommended;
            }

            if (outsResult < 3)
            {
                return CardValuationType.Unplayable;
            }
            else if (outsResult < 7 || bestHand.RankType < Logic.HandRankType.ThreeOfAKind)
            {
                return CardValuationType.Risky;
            }
            else if (outsResult < 12)
            {
                return CardValuationType.Recommended;
            }
            else
            {
                return CardValuationType.StronglyRecommended;
            }
        }

        public static CardValuationType River(Card firstCard, Card secondCard, IEnumerable<Card> comunityCards)
        {
            var handEvaluator = new HandEvaluator();

            var cards = new List<Card>();
            cards.AddRange(comunityCards);
            cards.Add(firstCard);
            cards.Add(secondCard);

            var playerCards = new List<Card>();
            playerCards.Add(firstCard);
            playerCards.Add(secondCard);

            var bestHand = handEvaluator.GetBestHand(cards);

            if (bestHand.RankType <= Logic.HandRankType.Pair)
            {
                return CardValuationType.Unplayable;
            }

            if (bestHand.RankType > Logic.HandRankType.Pair &&
                bestHand.RankType <= Logic.HandRankType.TwoPairs)
            {
                return CardValuationType.Risky;
            }

            if (bestHand.RankType > Logic.HandRankType.TwoPairs &&
                bestHand.RankType <= Logic.HandRankType.Straight)
            {
                return CardValuationType.Recommended;
            }

            if (bestHand.RankType > Logic.HandRankType.Straight)
            {
                return CardValuationType.StronglyRecommended;
            }

            return CardValuationType.StronglyRecommended;
        }
    }
}
