using RpgSoundboard.Models.Configs;

namespace RpgSoundboard.Services
{
    internal interface IConfigService
    {
        public AppConfig LoadConfig (string ConfigPath);
        void SaveConfig (string fILENAME_CONFIG_SOUNDBOARD, AppConfig config);
    }
}