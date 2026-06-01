# Auto Pod Reject

Auto Pod Reject automatically rejects Oxygen Not Included Printing Pod offers as soon as they become available.

The mod is disabled by default. Enable it from the ONI mods menu by opening this mod's **Options** button and turning on **Auto-reject Printing Pod offers**.

## Features

- Adds an **Options** button in the ONI mods menu through PLib.
- Exposes a single toggle named **Auto-reject Printing Pod offers**.
- Disabled by default for safe installation.
- Automatically rejects Printing Pod offers when enabled and the Printing Pod is operational.
- Pairs well with Printing Pod Recharge.
- No duplicant-count logic.
- No care-package filtering.
- No offer scoring, whitelists, or blacklists.
- No hard dependency on Printing Pod Recharge internals.

## Dependencies and packaging

PLib is supplied by this mod's build/package output. Users do **not** need to find, install, or subscribe to a separate PLib Workshop item.

The preferred release artifact is a single bundled DLL:

```text
AutoPodReject.dll
```

During the build, the project restores the PLib NuGet package and uses ILRepack to merge the mod assembly with the package's `PLib.dll`. The ONI, Harmony, Newtonsoft.Json, and Unity assemblies remain external game references and are not merged.

If a side-by-side fallback build is ever used, the install/package must ship `PLib.dll` next to `AutoPodReject.dll`. That layout has been tested and works in ONI, but users still should not install a separate PLib Workshop item.

## Build

Set either `ONI_DIR` to the Oxygen Not Included install directory or `ONI_MANAGED_DIR` directly to `OxygenNotIncluded_Data/Managed`, then run:

```bash
export ONI_DIR="/mnt/sda1/SteamLibrary/steamapps/common/OxygenNotIncluded"
./scripts/build.sh
```

The normal bundled build produces:

```text
bin/Release/AutoPodReject.dll
```

## Local install

To build and install into ONI's local mods folder on Linux, run:

```bash
export ONI_DIR="/mnt/sda1/SteamLibrary/steamapps/common/OxygenNotIncluded"
./scripts/install-local.sh
```

The script installs to:

```text
~/.config/unity3d/Klei/Oxygen Not Included/mods/Local/AutoPodReject/
```

For the normal bundled build, the local mod folder contains:

```text
AutoPodReject.dll
mod.yaml
mod_info.yaml
```

If a side-by-side fallback build leaves `bin/Release/PLib.dll` in the output folder, the install script also copies it so the local mod folder contains:

```text
AutoPodReject.dll
PLib.dll
mod.yaml
mod_info.yaml
```

A manually tested side-by-side install can also be created with:

```bash
mkdir -p "$HOME/.config/unity3d/Klei/Oxygen Not Included/mods/Local/AutoPodReject"

cp bin/Release/AutoPodReject.dll \
   bin/Release/PLib.dll \
   mod.yaml \
   mod_info.yaml \
   "$HOME/.config/unity3d/Klei/Oxygen Not Included/mods/Local/AutoPodReject/"
```

## Printing Pod Recharge compatibility

Auto Pod Reject always uses the normal `Telepad.RejectAll()` path instead of ending immigration directly. This preserves compatibility with mods that patch the reject flow, including Printing Pod Recharge.

When Printing Pod Recharge is installed, rejected offers can become Bio-Ink through that mod's normal behavior.

When Printing Pod Recharge is not installed, rejected offers are discarded. This is intentional.

## What this mod does not inspect

Auto Pod Reject does not inspect duplicants, care packages, or offer contents. If the option is enabled and the Printing Pod has available offers while operational, the entire offer set is rejected automatically.
