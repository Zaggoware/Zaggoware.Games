namespace Zaggoware.Games.Common
{
    public interface IGameRules
    {
        int MinPlayers { get; set; }

        int MaxPlayers { get; set; }

        IGameRules Copy();
    }
}