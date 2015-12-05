namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.Turing;
    using TexasHoldem.Logic.Players;

    public class TuringVsAlwaysRaisePlayer : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new TurinPlayer();
        private readonly IPlayer secondPlayer = new AlwaysRaiseDummyPlayer();

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
