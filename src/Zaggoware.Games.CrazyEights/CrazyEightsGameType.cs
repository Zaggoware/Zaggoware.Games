namespace Zaggoware.Games.CrazyEights
{
    using Zaggoware.Games.Common;

    public class CrazyEightsGameType : GameType<CrazyEightsGame, CrazyEightsGameRules>
    {
        public CrazyEightsGameType()
            : base("Crazy Eights")
        {
        }

        protected override CrazyEightsGame CreateGame(CrazyEightsGameRules gameRules)
        {
            return new CrazyEightsGame(gameRules);
        }
    }
}