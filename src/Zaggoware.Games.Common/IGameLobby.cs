namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;

    public interface IGameLobby
    {
        IGameConnection[] ActiveConnections { get; }

        ICollection<string> AdminIds { get; }

        DateTime CreatedOnUtc { get; }

        IGame? CurrentGame { get; }

        IGameType? CurrentGameType { get; }

        string Id { get; }

        bool IsPublic { get; }

        string Name { get; }

        void ChangeName(string name);

        bool ChangePassword(string currentPassword, string newPassword);

        void AddConnection(IGameConnection connection, string? password = null);

        void MakePrivate(string password);

        void MakePublic();

        void SetupGame(IGameType gameType, IGameRules gameRules);
    }
}