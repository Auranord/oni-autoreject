using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using UnityEngine;

namespace AutoPodReject
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(ModSettings));

            harmony.PatchAll();

            Debug.Log("[AutoPodReject] Loaded.");
        }
    }
}
