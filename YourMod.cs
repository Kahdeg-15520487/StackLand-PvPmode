using BepInEx;
using BerryLoaderNS;
using HarmonyLib;
using UnityEngine;

namespace YourMod
{
    [BepInPlugin("pvpmod", "PvP mode", "0.0.1")]
    [BepInDependency("BerryLoader")]
    public class Plugin : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource L;
        private Harmony HarmonyInstance;

        private void Awake()
        {
            L = Logger;
            L.LogInfo("awake pvpmod 1");
            HarmonyInstance = new Harmony("pvpmod");
            HarmonyInstance.PatchAll(typeof(Patches));
            L.LogInfo("awake pvpmod 2");
        }
    }

    public class Patches
    {

        [HarmonyPatch(typeof(GameCanvas), "Awake")]
        [HarmonyPostfix]
        public static void GCAPost()
        {
            MenuAPI.Init();
            var m = new GameObject().AddComponent<CustomMenu>();
            BerryLoader.L.LogInfo("hooking custom menu");
        }
    }
}