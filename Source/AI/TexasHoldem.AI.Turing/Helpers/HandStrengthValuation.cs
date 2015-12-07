// <copyright file="HandStrengthValuation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TexasHoldem.AI.Turing.Helpers
{
    using System.Collections.Generic;

    using Logic.Cards;
    using Logic.Helpers;

    public static class HandStrengthValuation
    {
        private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendations =
            {
                { 4, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1 },
                { 3, 4, 3, 3, 2, 2, 1, 1, 0, 0, 0, 0, 0 },
                { 3, 3, 4, 3, 2, 2, 1, 0, 0, 0, 0, 0, 0 },
                { 3, 3, 2, 4, 2, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 2, 2, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0 },
                { 2, 2, 1, 1, 2, 3, 2, 1, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 1, 3, 1, 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 1, 2, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }
            };

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

            var playerCards = new List<Card>();
            playerCards.Add(firstCard);
            playerCards.Add(secondCard);
            var percentageValuation = new PercentageValuation();
            var percentage = percentageValuation.CalculatePercentage(playerCards, cards);

            cards.Add(firstCard);
            cards.Add(secondCard);
            var bestHand = handEvaluator.GetBestHand(cards);

            if (bestHand.RankType >= Logic.HandRankType.FourOfAKind)
            {
                return CardValuationType.PlayItAllIn;
            }

            if (bestHand.RankType >= Logic.HandRankType.ThreeOfAKind)
            {
                return CardValuationType.Recommended;
            }

            if (percentage < 25)
            {
                return CardValuationType.Unplayable;
            }

            if (percentage < 45)
            {
                return CardValuationType.Risky;
            }

            if (percentage < 85)
            {
                return CardValuationType.Recommended;
            }

            return CardValuationType.StronglyRecommended;
        }

        public static CardValuationType Turn(Card firstCard, Card secondCard, IEnumerable<Card> comunityCards)
        {
            var handEvaluator = new HandEvaluator();

            var cards = new List<Card>();
            cards.AddRange(comunityCards);

            var playerCards = new List<Card>();
            playerCards.Add(firstCard);
            playerCards.Add(secondCard);

            var percentageValuation = new PercentageValuation();
            var percentage = percentageValuation.CalculatePercentage(playerCards, cards);

            cards.Add(firstCard);
            cards.Add(secondCard);

            var bestHand = handEvaluator.GetBestHand(cards);

            if (bestHand.RankType >= Logic.HandRankType.FourOfAKind)
            {
                return CardValuationType.PlayItAllIn;
            }

            if (percentage < 25)
            {
                return CardValuationType.Unplayable;
            }

            if (percentage < 45)
            {
                return CardValuationType.Risky;
            }

            if (percentage < 85)
            {
                return CardValuationType.Recommended;
            }

            return CardValuationType.StronglyRecommended;
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

            if (
                bestHand.RankType <= Logic.HandRankType.TwoPairs)
            {
                return CardValuationType.Risky;
            }

            if (bestHand.RankType > Logic.HandRankType.TwoPairs &&
                bestHand.RankType <= Logic.HandRankType.Straight)
            {
                return CardValuationType.Recommended;
            }

            if (bestHand.RankType > Logic.HandRankType.Flush)
            {
                return CardValuationType.StronglyRecommended;
            }

            return CardValuationType.StronglyRecommended;
        }
    }
}
