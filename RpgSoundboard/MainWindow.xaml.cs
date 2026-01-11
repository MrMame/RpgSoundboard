using Microsoft.Win32;
using RpgSoundboard.Models.Configs;
using RpgSoundboard.Services;
using RpgSoundboard.UI.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RpgSoundboard
{
    public partial class MainWindow : Window
    {
        private const string FILENAME_CONFIG_SOUNDBOARD = "soundboard_config.json";
        private AppConfig _config;

        private IConfigService _configService = new ConfigService ();

        public MainWindow ()
        {
            InitializeComponent ();
            _config = _configService.LoadConfig(FILENAME_CONFIG_SOUNDBOARD);
            BuildUIFromConfig ();
        }

        // -------------------------------
        // CONFIG LADEN
        // -------------------------------
        private void LoadConfig_Click (object sender, RoutedEventArgs e)
        {
            _config = _configService.LoadConfig (FILENAME_CONFIG_SOUNDBOARD);
            BuildUIFromConfig ();
        }

        //// -------------------------------
        //// CONFIG SPEICHERN
        //// -------------------------------
        private void SaveConfig_Click (object sender, RoutedEventArgs e)
        {
            _config = CreateConfigFromUI ();
            _configService.SaveConfig (FILENAME_CONFIG_SOUNDBOARD, _config);
        }


        // -------------------------------
        // CONFIG AUS UI AUFBAUEN
        // -------------------------------

        private AppConfig CreateConfigFromUI ()
        {
            _config.Collections.Clear ();
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
                        FilePath = SoundControlFactory.GetSoundFilePath (slot),
                        Loop = loopCheck.IsChecked == true
                    });
                }
                _config.Collections.Add (col);
            }
            return _config;
        }

        // -------------------------------
        // TAB LÖSCHEN
        // -------------------------------
        private void DelActCollection_Click (object sender, RoutedEventArgs e)
        {
            if (SoundCollectionsTabControl.Items.Count == 0) return;

            var result = MessageBox.Show ("Möchten Sie die aktuelle Sammlung wirklich löschen?",
                "Sammlung löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SoundCollectionsTabControl.Items.Remove (SoundCollectionsTabControl.SelectedItem);
            }
        }


        // -------------------------------
        // UI AUS CONFIG AUFBAUEN
        // -------------------------------
        private void BuildUIFromConfig ()
        {
            SoundCollectionsTabControl.Items.Clear ();

            foreach (var col in _config.Collections)
            {
                var tab = new TabItem
                {
                    Header = col.Name
                };

                StackPanel panel = new StackPanel ();

                foreach (var slot in col.Slots)
                {
                    panel.Children.Add (SoundControlFactory.CreateSoundControlFromConfig (slot));
                }

                tab.Content = panel;
                SoundCollectionsTabControl.Items.Add (tab);
            }
        }

        // -------------------------------
        // TAB HINZUFÜGEN
        // -------------------------------
        private void AddNewCollection_Click (object sender, RoutedEventArgs e)
        {
            var tab = new TabItem
            {
                Header = $"Neue Sammlung ({SoundCollectionsTabControl.Items.Count + 1})",
                Content = new StackPanel ()
            };

            SoundCollectionsTabControl.Items.Add (tab);
            SoundCollectionsTabControl.SelectedItem = tab;
        }


    }
}