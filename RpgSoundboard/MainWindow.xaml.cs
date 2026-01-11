using Microsoft.Win32;
using RpgSoundboard.Models.Configs;
using RpgSoundboard.Services;
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
        private AppConfig config;

        private IConfigService configService = new ConfigService ();

        public MainWindow ()
        {
            InitializeComponent ();
            config = configService.LoadConfig(FILENAME_CONFIG_SOUNDBOARD);
            BuildUIFromConfig ();
        }

        // -------------------------------
        // CONFIG LADEN
        // -------------------------------
        //private void LoadConfig ()
        //{
        //    if (!File.Exists (ConfigPath))
        //        return;

        //    try
        //    {
        //        string json = File.ReadAllText (ConfigPath);
        //        Config = JsonSerializer.Deserialize<AppConfig> (json);
        //    } catch
        //    {
        //        Config = new AppConfig ();
        //    }
        //}

        //// -------------------------------
        //// CONFIG SPEICHERN
        //// -------------------------------
        //private void SaveConfig ()
        //{
        //    Config.Collections.Clear ();

        //    foreach (TabItem tab in SoundCollectionsTabControl.Items)
        //    {
        //        var col = new SoundCollectionConfig
        //        {
        //            Name = tab.Header.ToString ()
        //        };

        //        var panel = (StackPanel)tab.Content;

        //        foreach (StackPanel slot in panel.Children)
        //        {
        //            var playBtn = (Button)slot.Children[0];
        //            var grid = (Grid)slot.Children[1];
        //            var loopCheck = (CheckBox)grid.Children[0];

        //            col.Slots.Add (new SoundSlotConfig
        //            {
        //                Title = playBtn.Content.ToString ().Replace ("▶ ", ""),
        //                FilePath = GetSoundFilePath (slot),
        //                Loop = loopCheck.IsChecked == true
        //            });
        //        }

        //        Config.Collections.Add (col);
        //    }

        //    string json = JsonSerializer.Serialize (Config, new JsonSerializerOptions { WriteIndented = true });
        //    File.WriteAllText (ConfigPath, json);
        //}

        private void SaveConfig ()
        {
            config.Collections.Clear ();
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
                config.Collections.Add (col);
            }
            configService.SaveConfig (FILENAME_CONFIG_SOUNDBOARD, config);
        }

        // -------------------------------
        // UI AUS CONFIG AUFBAUEN
        // -------------------------------
        private void BuildUIFromConfig ()
        {
            SoundCollectionsTabControl.Items.Clear ();

            foreach (var col in config.Collections)
            {
                var tab = new TabItem
                {
                    Header = col.Name
                };

                StackPanel panel = new StackPanel ();

                foreach (var slot in col.Slots)
                {
                    panel.Children.Add (CreateSoundControlFromConfig (slot));
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

            SaveConfig ();
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
                SaveConfig ();
            }
        }

        // -------------------------------
        // SOUND CONTROL AUS CONFIG
        // -------------------------------
        private UIElement CreateSoundControlFromConfig (SoundSlotConfig cfg)
        {
            var control = CreateSoundControl (cfg.Title);

            var container = (StackPanel)control;
            var playBtn = (Button)container.Children[0];
            var grid = (Grid)container.Children[1];
            var loopCheck = (CheckBox)grid.Children[0];

            loopCheck.IsChecked = cfg.Loop;

            if (!string.IsNullOrEmpty (cfg.FilePath))
                playBtn.Content = "▶ " + System.IO.Path.GetFileNameWithoutExtension (cfg.FilePath);

            SetSoundFilePath (container, cfg.FilePath);

            return control;
        }

        //// -------------------------------
        //// SOUND CONTROL ERZEUGEN
        //// -------------------------------
        //private UIElement CreateSoundControl (string defaultTitle)
        //{
        //    StackPanel container = new StackPanel { Margin = new Thickness (10), Width = 150 };
        //    MediaPlayer player = new MediaPlayer ();
        //    string selectedFilePath = "";

        //    Button playBtn = new Button
        //    {
        //        Content = "▶ " + defaultTitle,
        //        Height = 50,
        //        Background = Brushes.DarkRed,
        //        Foreground = Brushes.White
        //    };

        //    Grid controlsGrid = new Grid ();
        //    controlsGrid.RowDefinitions.Add (new RowDefinition ());
        //    controlsGrid.RowDefinitions.Add (new RowDefinition ());
        //    controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());
        //    controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());

        //    CheckBox loopCheck = new CheckBox
        //    {
        //        Content = "Loop",
        //        Foreground = Brushes.White,
        //        Margin = new Thickness (0, 5, 0, 5)
        //    };
        //    Grid.SetRow (loopCheck, 0);
        //    Grid.SetColumn (loopCheck, 0);
        //    controlsGrid.Children.Add (loopCheck);

        //    Button stopBtn = new Button { Content = "⏹ ", FontSize = 10 };
        //    Grid.SetRow (stopBtn, 0);
        //    Grid.SetColumn (stopBtn, 1);
        //    controlsGrid.Children.Add (stopBtn);

        //    Button fileBtn = new Button { Content = "Datei wählen...", FontSize = 10 };
        //    Grid.SetRow (fileBtn, 1);
        //    Grid.SetColumn (fileBtn, 0);
        //    controlsGrid.Children.Add (fileBtn);

        //    // Datei wählen
        //    fileBtn.Click += (s, e) =>
        //    {
        //        OpenFileDialog openFileDialog = new OpenFileDialog ();
        //        openFileDialog.Filter = "Audio Dateien|*.mp3;*.wav;*.m4a";

        //        if (openFileDialog.ShowDialog () == true)
        //        {
        //            selectedFilePath = openFileDialog.FileName;
        //            playBtn.Content = "▶ " + System.IO.Path.GetFileNameWithoutExtension (selectedFilePath);
        //            SetSoundFilePath (container, selectedFilePath);
        //            SaveConfig ();
        //        }
        //    };

        //    // Abspielen
        //    playBtn.Click += (s, e) =>
        //    {
        //        string path = GetSoundFilePath (container);
        //        if (string.IsNullOrEmpty (path)) return;

        //        player.Open (new Uri (path));
        //        player.Play ();
        //    };

        //    // Einstellungen ein-/ausblenden
        //    playBtn.MouseRightButtonUp += (s, e) =>
        //    {
        //        controlsGrid.Visibility = controlsGrid.Visibility == Visibility.Visible
        //            ? Visibility.Collapsed
        //            : Visibility.Visible;
        //    };

        //    // Stop
        //    stopBtn.Click += (s, e) => player.Stop ();

        //    // Loop
        //    player.MediaEnded += (s, e) =>
        //    {
        //        if (loopCheck.IsChecked == true)
        //        {
        //            player.Position = TimeSpan.Zero;
        //            player.Play ();
        //        }
        //    };


        //    container.Children.Add (playBtn);
        //    container.Children.Add (controlsGrid);

        //    return container;
        //}

        // -------------------------------
        // ATTACHED PROPERTY: FILEPATH
        // -------------------------------
        public static readonly DependencyProperty SoundFilePathProperty =
            DependencyProperty.RegisterAttached (
                "SoundFilePath",
                typeof (string),
                typeof (MainWindow),
                new PropertyMetadata ("")
            );

        public static void SetSoundFilePath (UIElement element, string value)
        {
            element.SetValue (SoundFilePathProperty, value);
        }

        public static string GetSoundFilePath (UIElement element)
        {
            return (string)element.GetValue (SoundFilePathProperty);
        }
    }
}