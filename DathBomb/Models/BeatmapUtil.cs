using IPA.Loader;
using System.Linq;
using Zenject;

namespace DathBomb.Models
{
    public class BeatmapUtil : IInitializable
    {
        public IDifficultyBeatmap Currentmap { get; private set; }
        public bool IsNoodle { get; private set; }
        public bool IsChroma { get; private set; }

        [Inject]
        public BeatmapUtil(IDifficultyBeatmap level)
        {
            this.Currentmap = level;
        }

        public static bool IsNoodleMap(IDifficultyBeatmap level)
        {
            // thanks kinsi
            if (PluginManager.EnabledPlugins.Any(x => x.Name == "NoodleExtensions")) {
                var isIsNoodleMap = SongCore.Collections.RetrieveDifficultyData(level)?
                    .additionalDifficultyData?
                    ._requirements?.Any(x => x == "Noodle Extensions") == true;
                return isIsNoodleMap;
            }
            else {
                return false;
            }
        }
        public static bool IsChromaMap(IDifficultyBeatmap level)
        {

            if (PluginManager.EnabledPlugins.Any(x => x.Name == "Chroma")) {
                var isIsNoodleMap = SongCore.Collections.RetrieveDifficultyData(level)?
                    .additionalDifficultyData?
                    ._requirements?.Any(x => x == "Chroma") == true;
                isIsNoodleMap = isIsNoodleMap || SongCore.Collections.RetrieveDifficultyData(level)?
                    .additionalDifficultyData?
                    ._suggestions?.Any(x => x == "Chroma") == true;
                return isIsNoodleMap;
            }
            else {
                return false;
            }
        }

        public void Initialize()
        {
            this.IsNoodle = IsNoodleMap(this.Currentmap);
            this.IsChroma = IsChromaMap(this.Currentmap);
            Plugin.Log.Debug($"Noodle?:{this.IsNoodle}");
            Plugin.Log.Debug($"Chroma?:{this.IsChroma}");
        }
    }
}
