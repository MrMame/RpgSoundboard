using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeroQuestSoundboard
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

        private UIElement CreateSoundControl (string defaultTitle)
        {
            StackPanel container = new StackPanel { Margin = new Thickness (10), Width = 150 };
            MediaPlayer player = new MediaPlayer ();
            string selectedFilePath = "";

            // Icon / Play Button
            Button playBtn = new Button { Content = "▶ " + defaultTitle, Height = 50, Background = Brushes.DarkRed, Foreground = Brushes.White };

            // Loop Checkbox (RadioButtons sind hier oft unpraktisch, eine Checkbox ist klarer)
            CheckBox loopCheck = new CheckBox { Content = "Loop", Foreground = Brushes.White, Margin = new Thickness (0, 5, 0, 5) };

            // File Dialog Button
            Button fileBtn = new Button { Content = "Datei wählen...", FontSize = 10 };

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
            container.Children.Add (loopCheck);
            container.Children.Add (fileBtn);

            return container;
        }
    }
}