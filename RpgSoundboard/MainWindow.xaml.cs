using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RpgSoundboard
{
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent ();
            // Erstellen wir testweise 4 Sound-Slots im Dungeon
            for (int i = 0; i < 4; i++)
            {
                DungeonPanel.Children.Add (CreateSoundControl ($"Sound {i + 1}"));
            }
        }

        private void AddNewCollection_Click(object sender, RoutedEventArgs e)
        {
            var newTabIdx = SoundCollectionsTabControl.Items.Add (new TabItem
            {
                Header = $"Neue Sammlung ({SoundCollectionsTabControl.Items.Count+1})",
                Foreground = Brushes.Black,
                Content = CreateSoundControl ("Neuer Sound")
            });
            SoundCollectionsTabControl.SelectedIndex = newTabIdx;
        }
        private void DelActCollection_Click (object sender, RoutedEventArgs e)
        {
            if (SoundCollectionsTabControl.Items.Count == 0) return;
            var result = MessageBox.Show ("Möchten Sie die aktuelle Sammlung wirklich löschen?", "Sammlung löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                SoundCollectionsTabControl.Items.Remove (SoundCollectionsTabControl.SelectedItem);
            }
        }
        private UIElement CreateSoundControl (string defaultTitle)
        {
            StackPanel container = new StackPanel { Margin = new Thickness (10), Width = 150 };
            MediaPlayer player = new MediaPlayer ();
            string selectedFilePath = "";

            // Icon / Play Button
            Button playBtn = new Button { Content = "▶ " + defaultTitle, Height = 50, Background = Brushes.DarkRed, Foreground = Brushes.White };
            
            Grid controlsGrid = new Grid ();
            controlsGrid.RowDefinitions.Add (new RowDefinition ());
            controlsGrid.RowDefinitions.Add (new RowDefinition ());
            controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());
            controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());

            // Loop Checkbox (RadioButtons sind hier oft unpraktisch, eine Checkbox ist klarer)
            CheckBox loopCheck = new CheckBox { Content = "Loop", Foreground = Brushes.White, Margin = new Thickness (0, 5, 0, 5) };
            Grid.SetRow (loopCheck, 0);
            Grid.SetColumn (loopCheck, 0);
            controlsGrid.Children.Add (loopCheck);

            // Stop Button
            Button stopBtn = new Button { Content = "⏹ ", FontSize = 10 };
            Grid.SetRow (stopBtn, 0);
            Grid.SetColumn (stopBtn, 1);
            controlsGrid.Children.Add (stopBtn);
            

            // File Dialog Button
            Button fileBtn = new Button { Content = "Datei wählen...", FontSize = 10 };
            Grid.SetRow (fileBtn, 1);
            Grid.SetColumn (fileBtn, 0);
            controlsGrid.Children.Add (fileBtn);

            // Logik: Datei wählen
            fileBtn.Click += (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog ();
                openFileDialog.Filter = "Audio Dateien|*.mp3;*.wav;*.m4a";
                if (openFileDialog.ShowDialog () == true)
                {
                    selectedFilePath = openFileDialog.FileName;
                    playBtn.Content = "▶ " + System.IO.Path.GetFileNameWithoutExtension (selectedFilePath);
                }
            };

            // Logik: Abspielen
            playBtn.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty (selectedFilePath)) return;

                player.Open (new Uri (selectedFilePath));
                player.Play ();
            };
            playBtn.MouseRightButtonUp += (s, e) =>
            {
                // Nur das Aktive Control anzeigen / verstecken
                controlsGrid.Visibility = controlsGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; 
            };

            // Logik: Stop
            stopBtn.Click += (s, e) =>
            {
                player.Stop ();
            };

            // Logik: Loop-Ende Event
            player.MediaEnded += (s, e) =>
            {
                if (loopCheck.IsChecked == true)
                {
                    player.Position = TimeSpan.Zero;
                    player.Play ();
                }
            };

            
            container.Children.Add (playBtn);
            container.Children.Add (controlsGrid);
            //container.Children.Add (stopBtn);
            //container.Children.Add (loopCheck);
            //container.Children.Add (fileBtn);

            return container;
        }
    }
}