namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using Logic.Players;
    using AI.TuringExtended;
    using TexasHoldem.AI.Turing;

    public class TuringVsExtended : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new TurinPlayer();
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
