# Auto Pod Reject

Auto Pod Reject automatically rejects Oxygen Not Included Printing Pod offers as soon as they become available.

The mod is disabled by default. Enable it from the ONI mod options menu when you want ready Printing Pod offers to be rejected automatically.

## Features

- Automatically rejects Printing Pod offers when enabled.
- Uses a simple mod options toggle named `Enabled`.
- Disabled by default for safe installation.
- Pairs well with Printing Pod Recharge.
- No duplicant-count logic.
- No care-package filtering.
- No offer scoring, whitelists, or blacklists.
- No hard dependency on Printing Pod Recharge.

## Printing Pod Recharge compatibility

Auto Pod Reject always uses the normal `Telepad.RejectAll()` path instead of ending immigration directly. This preserves compatibility with mods that patch the reject flow, including Printing Pod Recharge.

When Printing Pod Recharge is installed, rejected offers can become Bio-Ink through that mod's normal behavior.

When Printing Pod Recharge is not installed, rejected offers are discarded. This is intentional.

## What this mod does not inspect

Auto Pod Reject does not inspect duplicants, care packages, or offer contents. If the option is enabled and the Printing Pod has available offers while operational, the entire offer set is rejected automatically.
