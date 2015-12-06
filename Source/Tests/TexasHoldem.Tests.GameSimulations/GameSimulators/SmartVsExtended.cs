namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using Logic.Players;
    using AI.SmartPlayer;
    using AI.TuringExtended;

    public class SmartVsExtended : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmartPlayer();
        private readonly IPlayer secondPlayer = new TuringExtendedPlayer();

        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }
    }
}
