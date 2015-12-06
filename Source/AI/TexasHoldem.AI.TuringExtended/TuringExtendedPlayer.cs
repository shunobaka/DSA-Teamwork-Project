﻿namespace TexasHoldem.AI.TuringExtended
{
    using System;
    using System.Linq;
    using TexasHoldem.AI.TuringExtended.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    // TODO: This player is far far away from being smart!
    public class TuringExtendedPlayer : BasePlayer
    {
        private int counter = 0;
        public override string Name
        {
            get;
        } = "TuringExtendedPlayer_" + Guid.NewGuid();

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
            else if (context.RoundType == GameRoundType.Flop)
            {
                return this.GetActionForFlop(context);
            }
            else if (context.RoundType == GameRoundType.Turn)
            {
                return this.GetActionForTurn(context);
            }
            else if (context.RoundType == GameRoundType.River)
            {
                return this.GetActionForRiver(context);
            }

            return PlayerAction.CheckOrCall();
        }

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
                var smallBlindsTimes = RandomProvider.Next(1, 2);

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

                var smallBlindsTimes = RandomProvider.Next(3, 5);

                if (context.MoneyToCall <= context.MoneyLeft / 5)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(5, 8);
                return PlayerAction.Raise(context.SmallBlind * 5);
            }

            return PlayerAction.CheckOrCall();
        }

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

                // this one is not working but it is an idea :)
                if (context.PreviousRoundActions.ToArray()
                        .Any(s => s.Action.Type == PlayerActionType.Raise && s.PlayerName != this.Name))
                {
                    return PlayerAction.Raise(100000);
                }

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * 30);
            }

            return PlayerAction.CheckOrCall();
        }

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
                var smallBlindsTimes = RandomProvider.Next(1, 4);
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
                var smallBlindsTimes = RandomProvider.Next(6, 14);

                return PlayerAction.Raise(context.SmallBlind * 20);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * 30);
            }

            return PlayerAction.CheckOrCall();
        }

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
                var smallBlindsTimes = RandomProvider.Next(6, 14);

                return PlayerAction.Raise(context.SmallBlind * 20);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * 1000);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
