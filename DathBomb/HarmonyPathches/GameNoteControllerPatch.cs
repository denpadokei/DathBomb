using DathBomb.Models;
using HarmonyLib;

namespace DathBomb.HarmonyPathches
{
    [HarmonyPatch(typeof(GameNoteController), "Awake")]
    public class GameNoteControllerPatch
    {
        public static void Postfix(GameNoteController __instance)
        {
            _ = __instance.gameObject.AddComponent<DummyBomb>();
        }
    }
}
