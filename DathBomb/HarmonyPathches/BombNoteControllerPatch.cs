using HarmonyLib;
using UnityEngine;

namespace DathBomb.HarmonyPathches
{
    [HarmonyPatch(typeof(BombNoteController), "Awake")]
    public class BombNoteControllerPatch
    {
        public static MeshRenderer BombMesh { get; internal set; }
        public static void Postfix(BombNoteController __instance)
        {
            if (!BombMesh) {
                BombMesh = GameObject.Instantiate(__instance.GetComponentInChildren<MeshRenderer>());
                BombMesh.transform.SetParent(null, false);
                BombMesh.enabled = false;
            }
        }
    }
}
