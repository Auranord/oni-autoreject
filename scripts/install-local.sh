#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="${CONFIGURATION:-Release}"
MOD_DIR="${AUTO_POD_REJECT_MOD_DIR:-$HOME/.config/unity3d/Klei/Oxygen Not Included/mods/Local/AutoPodReject}"
OUTPUT_DIR="$ROOT_DIR/bin/$CONFIGURATION"

"$ROOT_DIR/scripts/build.sh"

mkdir -p "$MOD_DIR"
cp "$OUTPUT_DIR/AutoPodReject.dll" "$ROOT_DIR/mod.yaml" "$ROOT_DIR/mod_info.yaml" "$MOD_DIR/"

if [[ -f "$OUTPUT_DIR/PLib.dll" ]]; then
  cp "$OUTPUT_DIR/PLib.dll" "$MOD_DIR/"
  echo "Installed Auto Pod Reject with side-by-side PLib.dll to: $MOD_DIR"
else
  rm -f "$MOD_DIR/PLib.dll"
  echo "Installed Auto Pod Reject bundled DLL to: $MOD_DIR"
fi
