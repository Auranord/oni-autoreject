using HarmonyLib;
using UnityEngine;

namespace AutoPodReject.Patches
{
    [HarmonyPatch(typeof(Telepad), nameof(Telepad.Update))]
    public static class TelepadUpdatePatch
    {
        public static bool Prefix(Telepad __instance)
        {
            if (!ModSettings.Instance.Enabled)
                return true;

            if (Immigration.Instance == null)
                return true;

            if (!Immigration.Instance.ImmigrantsAvailable)
                return true;

            var operational = __instance.GetComponent<Operational>();
            if (operational == null || !operational.IsOperational)
                return true;

            Debug.Log("[AutoPodReject] Auto-rejecting Printing Pod offers.");

            __instance.RejectAll();

            return false;
        }
    }
}
