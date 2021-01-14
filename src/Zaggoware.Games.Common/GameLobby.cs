namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameLobby : IGameLobby
    {
        private readonly IList<IGameConnection> _activeConnections = new List<IGameConnection>();

        private GameLobby()
        {
        }

        public static GameLobby Create(string name, string? password = null)
        {
            return new GameLobby
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                HashedPassword = password
            };
        }

        public IGameConnection[] ActiveConnections => _activeConnections.ToArray();

        public ICollection<string> AdminIds { get; protected set; } = new List<string>();

        public DateTime CreatedOnUtc { get; protected set; } = DateTime.UtcNow;

        public IGame? CurrentGame { get; protected set; }

        public IGameType? CurrentGameType { get; protected set; }

        public string Id { get; protected set; } = string.Empty;

        public bool IsPublic => !string.IsNullOrEmpty(HashedPassword);

        public string Name { get; protected set; } = string.Empty;

        protected string? HashedPassword { get; set; }

        public virtual void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("New name cannot be empty.", nameof(newName));
            }

            Name = newName;
        }

        public virtual bool ChangePassword(string currentPassword, string newPassword)
        {
            if (IsPublic)
            {
                throw new InvalidOperationException(
                    $"The lobby is public. Please use {nameof(MakePrivate)}() instead.");
            }

            if (HashPassword(currentPassword) != HashedPassword)
            {
                return false;
            }

            SetPasswordInternal(newPassword, nameof(newPassword));
            return true;
        }

        public virtual void AddConnection(IGameConnection connection, string? password = null)
        {
            if (HashedPassword != null && HashedPassword != HashPassword(password ?? string.Empty))
            {
                return;
            }

            _activeConnections.Add(connection);
        }

        public virtual void MakePrivate(string password)
        {
            if (!IsPublic)
            {
                throw new InvalidOperationException(
                    $"The lobby is already private. Please use {nameof(ChangePassword)}() instead.");
            }

            SetPasswordInternal(password, nameof(password));
        }

        public virtual void MakePublic()
        {
            HashedPassword = null;
        }

        public void SetupGame(IGameType gameType, IGameRules gameRules)
        {
            var game = gameType.CreateGame(gameRules);
            CurrentGame?.Dispose();
            CurrentGame = game;
            CurrentGameType = gameType;
        }

        private static string HashPassword(string password)
        {
            return GamePasswordHasher.HashPassword(password);
        }

        private void SetPasswordInternal(string password, string passwordParamName)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty.", passwordParamName);
            }

            HashedPassword = HashPassword(password);
        }
    }
}