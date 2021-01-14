namespace Zaggoware.Games.Common
{
    public interface IGameType
    {
        string Name { get; }

        string UserFriendlyName { get; }

        IGame CreateGame(IGameRules gameRules);
    }
}