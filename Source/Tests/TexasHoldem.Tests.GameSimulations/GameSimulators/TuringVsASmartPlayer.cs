namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.AI.Turing;
    using TexasHoldem.Logic.Players;

    /// <summary>
    /// For performance profiling
    /// </summary>
    public class TuringVsASmartPlayer : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new TuringPlayer();
        private readonly IPlayer secondPlayer = new SmartPlayer();

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
