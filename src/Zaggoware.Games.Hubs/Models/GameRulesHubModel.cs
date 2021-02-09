namespace Zaggoware.Games.Hubs.Models
{
    using System.Collections.Generic;

    using Zaggoware.Games.Common;

    public class GameRulesHubModel : Dictionary<string, object?>
    {
        public GameRulesHubModel(IGameRules gameRules)
        {
            var properties = gameRules.GetType().GetProperties();
            foreach (var property in properties)
            {
                Add(property.Name, property.GetValue(gameRules));
            }
        }
    }
}