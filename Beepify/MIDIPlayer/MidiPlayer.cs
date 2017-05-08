using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beepify.MIDI;
using System.Threading;

namespace Beepify.MIDIPlayer
{
    class MidiPlayer
    {
        private Midi mid;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mid">Midi instance</param>
        public MidiPlayer(Midi mid)
        {
            this.mid = mid;
        }

        /// <summary>
        /// Plays the midi tracks
        /// </summary>
        public void Play()
        {
            // Mary, just delete when implementation starts
            Console.Beep(247, 400);
            Console.Beep(220, 400);
            Console.Beep(196, 400);
            Console.Beep(220, 400);
            Console.Beep(247, 400);
            Console.Beep(247, 400);
            Console.Beep(247, 800);

            Console.Beep(220, 400);
            Console.Beep(220, 400);
            Console.Beep(220, 800);

            Console.Beep(247, 400);
            Console.Beep(294, 400);
            Console.Beep(294, 800);

            Console.Beep(247, 400);
            Console.Beep(220, 400);
            Console.Beep(196, 400);
            Console.Beep(220, 400);
            Console.Beep(247, 400);
            Console.Beep(247, 400);
            Console.Beep(247, 800);

            Console.Beep(247, 400);
            Console.Beep(220, 400);
            Console.Beep(220, 400);
            Console.Beep(247, 400);
            Console.Beep(220, 400);
            Console.Beep(392, 400);
            // Tada
        }
    }


}
