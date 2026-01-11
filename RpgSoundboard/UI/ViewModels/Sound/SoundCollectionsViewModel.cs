using RpgSoundboard.Models.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RpgSoundboard.UI.ViewModels.Sound
{
    internal class SoundCollectionsViewModel : INotifyPropertyChanged
    {
        private SoundCollection _soundCollection;

        public SoundCollection SoundCollection
        {
            get => _soundCollection;
            set
            {
                if (_soundCollection != value)
                {
                    _soundCollection = value;
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoundCollection)));
                    OnPropertyChanged ();
                }
            }
        }

        public SoundCollectionsViewModel(SoundCollection soundCollection)
        {
            _soundCollection = soundCollection;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));
    }
}
