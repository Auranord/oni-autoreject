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
