namespace Zaggoware.Games.Common
{
    public interface IGameType
    {
        string Name { get; }

        string[] DefaultRulePresets { get; }

        IGameRules CreateDefaultGameRules(string? preset = null);

        IGame CreateGame(IGameRules gameRules);
    }
}