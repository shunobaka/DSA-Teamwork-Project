namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.TuringExtended;
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.Logic.Players;

    public class AlwaysInVsExtended : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AlwaysAllInDummyPlayer();
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
