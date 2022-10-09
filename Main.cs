using BepInEx;
using BerryLoaderNS;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PvPMod
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
            HarmonyInstance = new Harmony("pvpmod");
            HarmonyInstance.PatchAll(typeof(Patches));

            Datas.ModDir = Directory.GetParent(this.Info.Location).FullName;
            L.LogInfo($"Set mod dir to {Datas.ModDir}");
        }
    }

    public class Patches
    {
        [HarmonyPatch(typeof(GameCanvas), "Awake")]
        [HarmonyPostfix]
        public static void GCAPost()
        {
            //MenuAPI.Init();
            var L = BerryLoader.L;
            L.LogInfo($"Loading textures from {Path.Combine(Datas.ModDir, "textures.txt")} {File.Exists(Path.Combine(Datas.ModDir, "textures.txt"))}");
            foreach (var t in File.ReadAllLines(Path.Combine(Datas.ModDir, "textures.txt")).Select(l => l.Split('|')))
            {
                L.LogInfo($"Loading {t[0]}: {Path.Combine(Datas.ModDir, "UI", t[1])} {File.Exists(Path.Combine(Datas.ModDir, "UI", t[1]))}");
                var rawimg = File.ReadAllBytes(Path.Combine(Datas.ModDir, "UI", t[1]));
                L.LogInfo(rawimg.Length);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(rawimg);
                Datas.Textures.Add(t[0], tex);
                L.LogInfo($"Loaded {t[0]}");
            }
            var m = new GameObject().AddComponent<CustomMenu>();
            BerryLoader.L.LogInfo("hooking custom menu");
        }
    }

    class Datas
    {
        public static string ModDir;
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    }
}