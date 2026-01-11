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
    internal class SoundSlotViewModel : INotifyPropertyChanged
    {

        private SoundSlot _soundSlot;

        public string Title { 
            get => _soundSlot.Title;
            set {
                _soundSlot.Title = value;
                OnPropertyChanged ();
            }
        }
        public string FilePath
        {
            get => _soundSlot.FilePath;
            set
            {
                _soundSlot.FilePath = value;
                OnPropertyChanged ();
            }
        }
        public bool Loop
        {
            get => _soundSlot.Loop;
            set
            {
                _soundSlot.Loop = value;
                OnPropertyChanged ();
            }
        }

        public SoundSlotViewModel (SoundSlot soundSlot) {
            _soundSlot = soundSlot;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string name = null) => PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));

    }
}
