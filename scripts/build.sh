#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="${CONFIGURATION:-Release}"

if [[ -z "${ONI_MANAGED_DIR:-}" && -z "${ONI_DIR:-}" ]]; then
  cat >&2 <<'MSG'
Set ONI_MANAGED_DIR to OxygenNotIncluded_Data/Managed or ONI_DIR to the Oxygen Not Included install directory before building.
MSG
  exit 1
fi

dotnet build "$ROOT_DIR/AutoPodReject.csproj" -c "$CONFIGURATION"
