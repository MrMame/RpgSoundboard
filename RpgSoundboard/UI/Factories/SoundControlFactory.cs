using Microsoft.Win32;
using RpgSoundboard.Models.Configs;
using RpgSoundboard.Models.Sound;
using RpgSoundboard.ViewModels.Sound;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace RpgSoundboard.UI.Factories;

public class SoundControlFactory
{

         // -------------------------------
         // SOUND CONTROL ERZEUGEN
         // -------------------------------
        public static UIElement CreateSoundControl (SoundSlot soundSlot)
        {
        StackPanel container = new StackPanel { Margin = new Thickness (10), Width = 150 };
        container.DataContext = new SoundSlotViewModel(soundSlot);

        MediaPlayer player = new MediaPlayer ();
        string selectedFilePath = "";

        Button playBtn = new Button
        {
            Height = 50,
            Background = Brushes.DarkRed,
            Foreground = Brushes.White
        };
        playBtn.SetBinding (Button.ContentProperty, "Title");

        Grid controlsGrid = new Grid ();
        controlsGrid.RowDefinitions.Add (new RowDefinition ());
        controlsGrid.RowDefinitions.Add (new RowDefinition ());
        controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());
        controlsGrid.ColumnDefinitions.Add (new ColumnDefinition ());

        CheckBox loopCheck = new CheckBox
        {
            Content = "Loop",
            Foreground = Brushes.White,
            Margin = new Thickness (0, 5, 0, 5),
        };
        loopCheck.SetBinding (CheckBox.IsCheckedProperty,"Loop");

        Grid.SetRow (loopCheck, 0);
        Grid.SetColumn (loopCheck, 0);
        controlsGrid.Children.Add (loopCheck);

        Button stopBtn = new Button { Content = "⏹ ", FontSize = 10 };
        Grid.SetRow (stopBtn, 0);
        Grid.SetColumn (stopBtn, 1);
        controlsGrid.Children.Add (stopBtn);

        Button fileBtn = new Button { Content = "Datei wählen...", FontSize = 10 };
        Grid.SetRow (fileBtn, 1);
        Grid.SetColumn (fileBtn, 0);
        controlsGrid.Children.Add (fileBtn);

        // Datei wählen
        fileBtn.Click += (s, e) =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog ();
            openFileDialog.Filter = "Audio Dateien|*.mp3;*.wav;*.m4a";

            if (openFileDialog.ShowDialog () == true)
            {
                selectedFilePath = openFileDialog.FileName;
                playBtn.Content = System.IO.Path.GetFileNameWithoutExtension (selectedFilePath);
            }
        };

        // Abspielen
        playBtn.Click += (s, e) =>
        {
            string path = GetSoundFilePath (container);
            if (string.IsNullOrEmpty (path)) return;

            player.Open (new Uri (path));
            player.Play ();
        };

        // Einstellungen ein-/ausblenden
        playBtn.MouseRightButtonUp += (s, e) =>
        {
            controlsGrid.Visibility = controlsGrid.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        };

        // Stop
        stopBtn.Click += (s, e) => player.Stop ();

        // Loop
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

        return container;
    }

    // -------------------------------
    // SOUND CONTROL AUS CONFIG
    // -------------------------------
    public static UIElement CreateSoundControlFromConfig (SoundSlot soundSlot)
    {
      
        var control = CreateSoundControl (soundSlot);

        var container = (StackPanel)control;
        var playBtn = (Button)container.Children[0];
        var grid = (Grid)container.Children[1];
        var loopCheck = (CheckBox)grid.Children[0];

        loopCheck.IsChecked = soundSlot.Loop;

        if (!string.IsNullOrEmpty (soundSlot.FilePath))
            playBtn.Content = "▶ " + System.IO.Path.GetFileNameWithoutExtension (soundSlot.FilePath);

        SetSoundFilePath (container, soundSlot.FilePath);

        return control;
    }

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
