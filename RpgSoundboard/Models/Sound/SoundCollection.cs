using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgSoundboard.Models.Sound
{
    public class SoundCollection
    {
        public string Name { get; set; }
        public List<SoundSlot> Slots { get; set; } = new ();

    }
}
