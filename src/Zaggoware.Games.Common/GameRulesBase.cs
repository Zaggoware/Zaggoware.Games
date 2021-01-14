namespace Zaggoware.Games.Common
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Zaggoware.Games.Common.Converters;

    public class GameRulesBase : IGameRules
    {
        public int MaxPlayers { get; set; }

        public int MinPlayers { get; set; }

        public virtual IGameRules Copy()
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new JsonTimeSpanConverter()
                }
            };

            var json = JsonSerializer.Serialize(this, GetType(), options);
            var clone = JsonSerializer.Deserialize(json, GetType(), options) as IGameRules
                ?? throw new InvalidOperationException("Clone cannot be null.");
            return clone;
        }

    }
}