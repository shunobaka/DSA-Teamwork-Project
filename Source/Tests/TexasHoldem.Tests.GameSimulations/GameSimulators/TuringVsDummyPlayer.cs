namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.Turing;
    using TexasHoldem.Logic.Players;

    /// <summary>
    /// For performance profiling
    /// </summary>
    public class TuringVsDummyPlayer : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new TurinPlayer();
        private readonly IPlayer secondPlayer = new DummyPlayer();

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
