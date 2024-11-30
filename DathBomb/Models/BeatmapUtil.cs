using IPA.Loader;
using System.Linq;
using Zenject;

namespace DathBomb.Models
{
    public class BeatmapUtil : IInitializable
    {
        public BeatmapLevel Currentmap { get; private set; }
        public BeatmapKey Currentmapkey { get; private set; }
        public bool IsNoodle { get; private set; }
        public bool IsChroma { get; private set; }

        [Inject]
        public BeatmapUtil(GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            this.Currentmap = gameplayCoreSceneSetupData.beatmapLevel;
            this.Currentmapkey = gameplayCoreSceneSetupData.beatmapKey;
        }

        public static bool IsNoodleMap(BeatmapLevel level, BeatmapKey key)
        {
            // thanks kinsi
            if (PluginManager.EnabledPlugins.Any(x => x.Name == "NoodleExtensions"))
            {
                var isIsNoodleMap = SongCore.Collections.RetrieveDifficultyData(level, key)?
                    .additionalDifficultyData?
                    ._requirements?.Any(x => x == "Noodle Extensions") == true;
                return isIsNoodleMap;
            }
            else
            {
                return false;
            }
        }
        public static bool IsChromaMap(BeatmapLevel level, BeatmapKey key)
        {

            if (PluginManager.EnabledPlugins.Any(x => x.Name == "Chroma"))
            {
                var isIsNoodleMap = SongCore.Collections.RetrieveDifficultyData(level, key)?
                    .additionalDifficultyData?
                    ._requirements?.Any(x => x == "Chroma") == true;
                isIsNoodleMap = isIsNoodleMap || SongCore.Collections.RetrieveDifficultyData(level, key)?
                    .additionalDifficultyData?
                    ._suggestions?.Any(x => x == "Chroma") == true;
                return isIsNoodleMap;
            }
            else
            {
                return false;
            }
        }

        public void Initialize()
        {
            this.IsNoodle = IsNoodleMap(this.Currentmap, this.Currentmapkey);
            this.IsChroma = IsChromaMap(this.Currentmap, this.Currentmapkey);
            Plugin.Log.Debug($"Noodle?:{this.IsNoodle}");
            Plugin.Log.Debug($"Chroma?:{this.IsChroma}");
        }
    }
}
