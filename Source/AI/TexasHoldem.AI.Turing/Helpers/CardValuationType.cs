namespace TexasHoldem.AI.Turing.Helpers
{
    public enum CardValuationType
    {
        Unplayable = 0,
        NotRecommended = 1000,
        Risky = 2000,
        Recommended = 3000,
        StronglyRecommended = 4000,
        PlayItAllIn = 5000
    }
}
