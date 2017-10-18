using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beepify.MIDIPlayer
{
    class PlayerNote
    {
        public int Frequency { get; private set; }
        public int Duration { get; private set; }
        public ushort Volume { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="frequency">Frequency at which the note plays</param>
        /// <param name="duration">Duration the node should play</param>
        /// <param name="volume">Volume at which note plays</param>
        public PlayerNote(int frequency, int duration, ushort volume = ushort.MaxValue)
        {
            Frequency = frequency;
            Duration = duration;
            Volume = volume;
        }

        /// <summary>
        /// Overload constructor for unknown durations
        /// and frequencies
        /// </summary>
        /// <param name="midiTone">Midi tone (value between 0 and 128)</param>
        /// <param name="startIndex">The index of the node where it was found</param>
        /// <param name="endIndex">The index of a node on the same channel that signaled for turning off</param>
        /// <param name="tempo">The tempo of the midi file at node start</param>
        /// <param name="volume">Volume at which note plays</param>
        public PlayerNote(byte midiTone, int startIndex, int endIndex, int tempo, ushort volume = ushort.MaxValue)
        {
            Duration = (int)((endIndex - startIndex) * (60000000000.0 / tempo)) -20;
            Frequency = MidiNoteToFrequency(midiTone);
            Volume = volume;
        }

        /// <summary>
        /// Converts a MIDI note to a frequency
        /// that can be played with Beep
        /// *NOTE*: note is rounded to nearest integer
        /// </summary>
        /// <param name="note">Note number</param>
        /// <returns>Frequency in Hz</returns>
        public static int MidiNoteToFrequency(byte note)
        {
            // Credits: http://subsynth.sourceforge.net/midinote2freq.html
            //return (int)Math.Round((440.0 / 32.0) * (Math.Pow(2, ((note - 9) / 12))));
            return (int)Math.Round(440.0 * Math.Pow(2, ((note - 69) / 12.0)));
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append($"Frequency: {Frequency}, ")
                .Append($"Duration: {Duration}, ")
                .Append($"Volume: {Volume}")
                .ToString();
        }
    }
}
