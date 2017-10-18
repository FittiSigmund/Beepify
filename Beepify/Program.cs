using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Beepify.MIDI;
using Beepify.MIDIPlayer;
using System.IO;

namespace Beepify
{
    class Program
    {
        static void Main(string[] args)
        {
            Midi mid = new Midi("smb.mid");
            MidiPlayer player = new MidiPlayer(mid);
            //player.Play();

            

            Console.ReadKey();
        }

    }
}
