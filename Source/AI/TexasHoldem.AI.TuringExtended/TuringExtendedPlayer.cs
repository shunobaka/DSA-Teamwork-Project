using System.Collections.Generic;
using TexasHoldem.Logic.Cards;

namespace TexasHoldem.AI.TuringExtended
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
        private int raisesOnPreFlop = 0;
        private int raisesOnFlop = 0;
        private int raisesOnTurn = 0;
        private int raisesOnRiver = 0;

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
                    if (this.raisesOnPreFlop < 1)
                    {
                        this.raisesOnPreFlop += 1;
                        return PlayerAction.Raise(context.SmallBlind * 2);
                    }
                }

                if (context.MoneyToCall <= context.MoneyLeft / 5)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                if (this.raisesOnPreFlop < 1)
                {
                    this.raisesOnPreFlop += 1;
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
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

                return PlayerAction.Fold();
            }

            if (playHand == CardValuationType.Risky)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            HandEvaluatorExtension handEvaluator = new HandEvaluatorExtension();
            var cards = new List<Card>();
            cards.AddRange(this.CommunityCards);
            cards.Add(this.FirstCard);
            cards.Add(this.SecondCard);
            var handsInCommunity = handEvaluator.GetBestHandForFlop(cards);

            if (handsInCommunity.RankType == HandRankType.Pair)
            {
                if (this.FirstCard.Type < CardType.Ten && this.SecondCard.Type < CardType.Ten)
                {
                    return PlayerAction.Fold();
                }
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(1, 14);
                if (context.CanCheck)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 10)
                    {
                        return PlayerAction.CheckOrCall();
                    }

                    return PlayerAction.Fold();
                }

                if (this.raisesOnFlop < 2)
                {
                    this.raisesOnPreFlop += 1;
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                if (this.raisesOnFlop < 1)
                {
                    this.raisesOnPreFlop += 1;
                    return PlayerAction.Raise(context.SmallBlind * 4);
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetActionForTurn(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Turn(this.FirstCard, this.SecondCard, this.CommunityCards);

            if (playHand == CardValuationType.Unplayable)
            {
                return PlayerAction.Fold();
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
                if (this.raisesOnTurn < 1)
                {
                    this.raisesOnTurn += 1;
                    return PlayerAction.Raise(context.SmallBlind * 4);
                }
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                if (this.raisesOnTurn < 1)
                {
                    this.raisesOnTurn += 1;
                    var smallBlindsTimes = RandomProvider.Next(8, 18);

                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.Raise(context.SmallBlind * 2);
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
            }

            if (playHand == CardValuationType.Risky)
            {
                return PlayerAction.CheckOrCall();
            }

            if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);

                if (this.raisesOnRiver < 3)
                {
                    this.raisesOnRiver += 1;
                    return PlayerAction.Raise(200);
                }
            }

            if (playHand == CardValuationType.StronglyRecommended)
            {
                if (this.raisesOnRiver >= 1)
                {
                    this.raisesOnRiver++;
                    return PlayerAction.Raise(10000);
                }

                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
