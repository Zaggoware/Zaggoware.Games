namespace Zaggoware.Games.CrazyEights
{
    using Zaggoware.Games.Common;

    public class CrazyEightsGameType : GameType<CrazyEightsGame, CrazyEightsGameRules>
    {
        public override string Name => "CrazyEights";

        public override string[] DefaultRulePresets => new[]
        {
            "Default",
            "Pesten"
        };

        protected override CrazyEightsGame CreateGame(CrazyEightsGameRules gameRules)
        {
            return new CrazyEightsGame(gameRules);
        }

        public override IGameRules CreateDefaultGameRules(string? preset = null)
        {
            return new CrazyEightsGameRules();
        }
    }
}