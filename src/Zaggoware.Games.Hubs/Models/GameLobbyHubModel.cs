namespace Zaggoware.Games.Hubs.Models
{
    using System;

    using Zaggoware.Games.Common;

    public class GameLobbyHubModel
    {
        public GameLobbyHubModel(IGameLobby lobby)
        {
            Id = lobby.Id;
            Name = lobby.Name;
            ActiveConnections = lobby.ActiveConnections.Length;
            CreatedOnUtc = lobby.CreatedOnUtc;
            CurrentGameId = lobby.CurrentGame?.Id;
            SelectedGameType = lobby.SelectedGameType?.Name;
            SelectedGameRules = lobby.SelectedGameRules != null ? new GameRulesHubModel(lobby.SelectedGameRules) : null;
            IsPrivate = lobby.IsPrivate;
        }

        public int ActiveConnections { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public string? CurrentGameId { get; set; }

        public string Id { get; set; }

        public bool IsPrivate { get; set; }

        public string Name { get; set; }

        public GameRulesHubModel? SelectedGameRules { get; set; }

        public string? SelectedGameType { get; set; }
    }
}