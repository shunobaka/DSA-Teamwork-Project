namespace TexasHoldem.AI.Turing
{
    using System;
    using System.Linq;

    using TexasHoldem.AI.Turing.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    // TODO: This player is far far away from being smart!
    public class TurinPlayer : BasePlayer
    {
        public override string Name
        {
            get;
        } = "TuringPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
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
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (playHand == CardValuationType.Risky)
            {
                var smallBlindsTimes = RandomProvider.Next(1, 8);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(int.MaxValue);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetActionForFlop(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Flop(this.FirstCard, this.SecondCard, this.CommunityCards);
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
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetActionForTurn(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Turn(this.FirstCard, this.SecondCard);
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
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetActionForRiver(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.River(this.FirstCard, this.SecondCard);
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
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                var smallBlindsTimes = RandomProvider.Next(14, 28);
                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
