// <copyright file="TurinPlayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TexasHoldem.AI.Turing
{
    using System;
    using System.Linq;

    using Helpers;
    using Logic;
    using Logic.Extensions;
    using Logic.Players;

    /// <summary>
    ///  Turing Player inherits Base player
    /// </summary>
    public class TurinPlayer : BasePlayer
    {
        public override string Name { get; } = "TuringPlayer_" + Guid.NewGuid();

        /// <summary>
        ///     Returns Player actions based on the context
        /// </summary>
        /// <param name="context">Information for the game</param>
        /// <returns>The player action</returns>
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.MoneyLeft <= 0)
            {
                return PlayerAction.CheckOrCall();
            }

            if (context.RoundType == GameRoundType.PreFlop)
            {
                return this.GetActionForPreFlop(context);
            }

            if (context.RoundType == GameRoundType.Flop)
            {
                return this.GetActionForFlop(context);
            }

            if (context.RoundType == GameRoundType.Turn)
            {
                return this.GetActionForTurn(context);
            }

            return this.GetActionForRiver(context);
        }

        /// <summary>
        ///     Returns Players action for the Preflop;
        /// </summary>
        /// <param name="context">the game context</param>
        /// <returns>The player's descision</returns>
        private PlayerAction GetActionForPreFlop(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
            if (playHand == CardValuationType.Unplayable)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.Risky)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (context.IsAllIn)
                {
                    return PlayerAction.Fold();
                }

                if (context.CurrentPot > context.MoneyLeft)
                {
                    return PlayerAction.Fold();
                }

                if (context.MoneyLeft <= context.SmallBlind * 5)
                {
                    return PlayerAction.Fold();
                }

                return PlayerAction.CheckOrCall();
            }

            if (playHand == CardValuationType.Recommended)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (context.MoneyLeft < context.SmallBlind * 10)
                {
                    return PlayerAction.Raise(10000);
                }

                if (context.MoneyToCall <= context.MoneyLeft / 5)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 5);
            }

            return PlayerAction.CheckOrCall();
        }

        /// <summary>
        ///     Returns Players action for the Flop;
        /// </summary>
        /// <param name="context">the game context</param>
        /// <returns>The player's descision</returns>
        private PlayerAction GetActionForFlop(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Flop(this.FirstCard, this.SecondCard, this.CommunityCards);
            if (playHand == CardValuationType.PlayItAllIn)
            {
                return PlayerAction.Raise(10000000);
            }

            if (playHand == CardValuationType.Unplayable)
            {

                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (playHand == CardValuationType.Risky)
            {
                var smallBlindsTimes = RandomProvider.Next(1, 8);
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);
                if (context.CanCheck)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 10)
                    {
                        return PlayerAction.CheckOrCall();
                    }

                    return PlayerAction.Fold();
                }

                if (context.PreviousRoundActions.ToArray()
                        .Any(s => s.Action.Type == PlayerActionType.Raise && s.PlayerName != this.Name))
                {
                    return PlayerAction.Raise(100000);
                }

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 30);
            }

            return PlayerAction.CheckOrCall();
        }

        /// <summary>
        ///     Returns Players action for the Turn;
        /// </summary>
        /// <param name="context">the game context</param>
        /// <returns>The player's descision</returns>
        private PlayerAction GetActionForTurn(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Turn(this.FirstCard, this.SecondCard, this.CommunityCards);
            if (playHand == CardValuationType.PlayItAllIn)
            {
                return PlayerAction.Raise(10000000);
            }

            if (playHand == CardValuationType.Unplayable)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (playHand == CardValuationType.Risky)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (context.MoneyToCall <= context.MoneyLeft / 20)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.Recommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 20);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 30);
            }

            return PlayerAction.CheckOrCall();
        }

        /// <summary>
        ///     Returns Players action for the River;
        /// </summary>
        /// <param name="context">the game context</param>
        /// <returns>The player's descision</returns>
        private PlayerAction GetActionForRiver(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.River(this.FirstCard, this.SecondCard, this.CommunityCards);
            if (playHand == CardValuationType.Unplayable)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (playHand == CardValuationType.Risky)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (context.MoneyToCall <= context.MoneyLeft / 20)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.Recommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 20);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                return PlayerAction.Raise(context.SmallBlind * 1000);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
