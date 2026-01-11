using RpgSoundboard.Models.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RpgSoundboard.ViewModels.Sound
{
    internal class SoundCollectionsViewModel : INotifyPropertyChanged
    {
        private SoundCollection _soundCollection = new SoundCollection();

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));
    }
}
