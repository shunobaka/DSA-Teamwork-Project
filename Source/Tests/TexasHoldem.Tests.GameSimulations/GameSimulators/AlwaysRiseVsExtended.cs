namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.TuringExtended;
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.Logic.Players;

    public class AlwaysRiseVsExtended : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AlwaysRaiseDummyPlayer();
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
