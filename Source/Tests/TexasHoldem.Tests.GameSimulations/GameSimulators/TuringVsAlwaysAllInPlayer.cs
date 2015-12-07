namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.Turing;
    using TexasHoldem.Logic.Players;

    public class TuringVsAlwaysAllInPlayer : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new TuringPlayer();
        private readonly IPlayer secondPlayer = new AlwaysAllInDummyPlayer();

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
