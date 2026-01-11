using RpgSoundboard.Models.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RpgSoundboard.Services
{
    internal class ConfigService : IConfigService
    {
        private AppConfig Config = new AppConfig ();


        public AppConfig LoadConfig (string ConfigPath)
        {
            if (!File.Exists (ConfigPath))
                return new AppConfig (); ;

            try
            {
                string json = File.ReadAllText (ConfigPath);
                return JsonSerializer.Deserialize<AppConfig> (json);
            } catch
            {
                return new AppConfig ();
            }
        }

        // -------------------------------
        // CONFIG SPEICHERN
        // -------------------------------
        private void SaveConfig ()
        {
            Config.Collections.Clear ();

            foreach (TabItem tab in SoundCollectionsTabControl.Items)
            {
                var col = new SoundCollectionConfig
                {
                    Name = tab.Header.ToString ()
                };

                var panel = (StackPanel)tab.Content;

                foreach (StackPanel slot in panel.Children)
                {
                    var playBtn = (Button)slot.Children[0];
                    var grid = (Grid)slot.Children[1];
                    var loopCheck = (CheckBox)grid.Children[0];

                    col.Slots.Add (new SoundSlotConfig
                    {
                        Title = playBtn.Content.ToString ().Replace ("▶ ", ""),
                        FilePath = GetSoundFilePath (slot),
                        Loop = loopCheck.IsChecked == true
                    });
                }

                Config.Collections.Add (col);
            }

            string json = JsonSerializer.Serialize (Config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText (ConfigPath, json);
        }

    }
}
