# Codex handoff: Auto Pod Reject mod for Oxygen Not Included

## Goal

Create a fresh Oxygen Not Included mod repo called:

**Auto Pod Reject**

The mod should add a simple ONI mod options menu setting. When enabled, it automatically rejects all Printing Pod offers as soon as the Printing Pod becomes ready.

This is primarily intended to work well together with **Printing Pod Recharge**, because that mod can turn rejected offers into Bio-Ink. However, this mod must also work without Printing Pod Recharge. If Printing Pod Recharge is not installed, the offers are simply rejected/lost. That behavior is acceptable and intentional.

## Desired behavior

When the Printing Pod has new offers ready:

1. Check whether the mod setting `Enabled` is true.
2. Check whether `Immigration.Instance.ImmigrantsAvailable` is true.
3. Check whether the current `Telepad` is operational.
4. Call `__instance.RejectAll()`.
5. Return `false` from the Harmony prefix so vanilla `Telepad.Update()` does not also open the pod or show the ready status on that frame.

Do not add any logic about duplicant count, offer contents, care packages, or Printing Pod Recharge detection.

## Non-goals

Do not add:

* duplicant-count thresholds
* care-package filtering
* auto-accepting care packages
* offer scoring
* whitelists or blacklists
* direct dependency on Printing Pod Recharge
* Bio-Ink logic
* custom side-screen buttons
* new buildings, items, or resources

This mod should only trigger the existing reject path automatically.

## Settings

Use a mod options menu toggle.

Preferred: use **PeterHan.PLib Options** if practical.

Expose this setting:

```csharp
Enabled: bool = false
```

Default must be:

```json
{
  "Enabled": false
}
```

Reason: safe default. The mod should never start rejecting offers automatically until the user explicitly enables it.

## User-facing options text

Option title:

```text
Auto-reject Printing Pod offers
```

Tooltip:

```text
Automatically rejects all Printing Pod offers as soon as they become available. If Printing Pod Recharge is installed, this can produce Bio-Ink. Without Printing Pod Recharge, offers are simply discarded.
```

## Main Harmony patch

Patch `Telepad.Update()` as a prefix.

Suggested core logic:

```csharp
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

            // Skip vanilla Telepad.Update this frame so it does not open/show the ready status.
            return false;
        }
    }
}
```

## Why call `Telepad.RejectAll()`

Do not call `Immigration.Instance.EndImmigration()` directly.

Call:

```csharp
__instance.RejectAll();
```

Reason: other mods can patch `Telepad.RejectAll()`. Printing Pod Recharge does this to produce Bio-Ink from rejected offers. Calling the normal method keeps compatibility clean.

## Mod entry point

Create a normal ONI Harmony mod entry point:

```csharp
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
```

Adjust if the project’s PLib setup requires slightly different boilerplate.

## Mod settings class

Suggested PLib-style settings class:

```csharp
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AutoPodReject
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("Auto Pod Reject")]
    public sealed class ModSettings
    {
        public static ModSettings Instance => POptions.ReadSettings<ModSettings>() ?? new ModSettings();

        [JsonProperty]
        [Option(
            "Auto-reject Printing Pod offers",
            "Automatically rejects all Printing Pod offers as soon as they become available. If Printing Pod Recharge is installed, this can produce Bio-Ink. Without Printing Pod Recharge, offers are simply discarded."
        )]
        public bool Enabled { get; set; } = false;
    }
}
```

If repeatedly reading settings every `Telepad.Update()` is undesirable, cache the settings on load and refresh when options are changed if PLib supports that cleanly. Keep it simple for v1.

## Suggested repo structure

```text
AutoPodReject/
├─ README.md
├─ AutoPodReject.csproj
├─ mod_info.yaml
├─ mod.yaml
├─ src/
│  ├─ Mod.cs
│  ├─ ModSettings.cs
│  └─ Patches/
│     └─ TelepadUpdatePatch.cs
└─ scripts/
   └─ build.sh
```

## README requirements

Document clearly:

* This mod automatically rejects Printing Pod offers.
* It has a mod options menu toggle.
* The default is disabled.
* It is intended to pair nicely with Printing Pod Recharge.
* If Printing Pod Recharge is installed, rejected offers should produce Bio-Ink through that mod’s normal behavior.
* If Printing Pod Recharge is not installed, offers are simply discarded.
* This behavior is intentional.
* The mod does not inspect dupes, care packages, or offer contents.

Suggested README wording:

```markdown
# Auto Pod Reject

Auto Pod Reject automatically rejects Printing Pod offers as soon as they become available.

This is useful if you dislike managing Printing Pod offers manually. It pairs especially well with Printing Pod Recharge, because rejected offers can become Bio-Ink.

If Printing Pod Recharge is not installed, this mod will still work, but rejected offers are simply lost.

The mod is disabled by default. Enable it from the ONI mod options menu.

## Features

- Automatically rejects Printing Pod offers.
- Uses a simple mod options toggle.
- No duplicant-count logic.
- No care-package filtering.
- No hard dependency on Printing Pod Recharge.

## Recommended use

Install alongside Printing Pod Recharge if you want rejected pod offers to become Bio-Ink.

Without Printing Pod Recharge, this mod intentionally discards Printing Pod offers.
```

## Acceptance criteria

The mod is done when:

1. ONI loads the mod without errors.
2. The mod appears in the ONI mod options menu.
3. The options menu has an `Enabled` toggle.
4. With `Enabled = false`, the Printing Pod behaves normally.
5. With `Enabled = true`, ready Printing Pod offers are automatically rejected.
6. The pod does not remain open waiting for manual interaction.
7. If Printing Pod Recharge is installed, its normal Bio-Ink refund behavior is triggered via the `Telepad.RejectAll()` path.
8. If Printing Pod Recharge is not installed, offers are still rejected/lost.
9. No duplicant-count logic exists.
10. No offer-selection or care-package logic exists.
11. The mod has no hard dependency on Printing Pod Recharge.

## Implementation notes

Patch `Telepad.Update()` as a prefix, not the immigrant selection screen.

Only act when:

```csharp
Immigration.Instance.ImmigrantsAvailable == true
```

After calling `RejectAll()`, return `false` from the prefix.

This should prevent the regular ready state from opening the portal or showing the normal pod-ready status on that frame.

Keep logging minimal:

* log once on mod load
* log once when auto-reject happens

Do not spam logs every update tick.
