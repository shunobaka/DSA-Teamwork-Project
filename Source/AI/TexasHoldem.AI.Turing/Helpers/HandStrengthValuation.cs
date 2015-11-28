namespace TexasHoldem.AI.Turing.Helpers
{
    using System;
    using TexasHoldem.Logic.Cards;

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
                    return CardValuationType.StronglyRemommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        // Feeling lucky punk!
        public static CardValuationType Flop(Card firstCard, Card secondCard)
        {
            var random = new Random();

            switch (random.Next(0, 5))
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
                    return CardValuationType.StronglyRemommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        // Feeling lucky punk!
        public static CardValuationType Turn(Card firstCard, Card secondCard)
        {
            var random = new Random();

            switch (random.Next(0, 5))
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
                    return CardValuationType.StronglyRemommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        // Feeling lucky punk!
        public static CardValuationType River(Card firstCard, Card secondCard)
        {
            var random = new Random();

            switch (random.Next(0, 5))
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
                    return CardValuationType.StronglyRemommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }
    }
}
