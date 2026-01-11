namespace RpgSoundboard.Models.Configs
{
    public class SoundCollectionConfig
    {
        public string Name { get; set; }
        public List<SoundSlotConfig> Slots { get; set; } = new ();
    }
}