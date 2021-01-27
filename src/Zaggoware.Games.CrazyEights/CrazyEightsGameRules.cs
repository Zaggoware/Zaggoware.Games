namespace Zaggoware.Games.CrazyEights
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.Common.Converters;

    public class CrazyEightsGameRules : TurnBasedCardGameRules
    {
        private const string RulesFolderPath = @"RulePresets\"+ nameof(CrazyEightsGame);
        private const string RulesFilePathFormat = "{0}.json";

        private static readonly ConcurrentDictionary<string, CrazyEightsGameRules> Presets
            = new ConcurrentDictionary<string, CrazyEightsGameRules>();

        // TODO: Make a generic 'RulesParser' class with configurable paths.
        public static CrazyEightsGameRules Create(string preset = "Default")
        {
            if (string.IsNullOrEmpty(preset))
            {
                return new CrazyEightsGameRules();
            }

            // TODO: Move preset logic to a separate manager.
            if (Presets.ContainsKey(preset))
            {
                return (CrazyEightsGameRules)Presets[preset].Copy();
            }

            // TODO: Don't directly use the preset in the path, needs validation first.
            var workingDir = Path.GetDirectoryName(typeof(CrazyEightsGameRules).Assembly.Location) ?? string.Empty;
            var path = Path.Combine(workingDir, RulesFolderPath, string.Format(RulesFilePathFormat, preset));
            if (!File.Exists(path))
            {
                return new CrazyEightsGameRules();
            }

            try
            {
                var json = File.ReadAllText(path);
                var jsonOptions = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(),
                        new JsonTimeSpanConverter()
                    }
                };

                var rules = JsonSerializer.Deserialize<CrazyEightsGameRules>(json, jsonOptions);
                if (rules != null)
                {
                    Presets.TryAdd(preset, rules);
                }

                return rules?.Copy() as CrazyEightsGameRules ?? new CrazyEightsGameRules();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new CrazyEightsGameRules();
            }
        }

        public bool AllowColorChangerCardEnding { get; set; }

        public bool AllowMandatoryDrawCardEnding { get; set; }

        public bool AllowReversingCardEnding { get; set; }

        public bool AllowSingleTurnStackableCardEnding { get; set; }

        public CardStackingMode? ColorChangerCardStackingMode { get; set; }

        public CardRank[] ColorChangerCards { get; set; } = Array.Empty<CardRank>();

        public ColorChangerMode ColorChangerMode { get; set; }

        public CardStackingMode DefaultStackingMode { get; set; }

        public bool EliminationMode { get; set; }

        public Dictionary<CardRank, int> MandatoryDrawCards { get; set; } = new Dictionary<CardRank, int>();

        public CardRank[] ReversingCards { get; set; } = Array.Empty<CardRank>();

        public CardRank[] SingleTurnStackableCards { get; set; } = Array.Empty<CardRank>();

        public int StartingCardsPerPlayer { get; set; }

        public CardRank[] SkipNextTurnCards { get; set; } = Array.Empty<CardRank>();
    }
}