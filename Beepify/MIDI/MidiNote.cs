using System;
using System.Text;

namespace Beepify.MIDI
{
    public struct MidiNote
    {
        public byte Note;
        public byte Velocity;
        public byte Channel;
        public Events.Midi NoteType;
        public int Ticks;

        public MidiNote(byte note, byte velocity, byte channel, int ticks, Events.Midi noteType)
        {
            Note = note;
            Velocity = velocity;
            Channel = channel;
            Ticks = ticks;
            NoteType = noteType;
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append($"Note: {Note.ToString("X")}, ")
                .Append($"Velocity: {Velocity.ToString("X")}, ")
                .Append($"Channel: {Channel.ToString("X")}, ")
                .Append($"Ticks: {Channel.ToString("X")}, ")
                .Append($"Node Type: {Enum.GetName(typeof(Events.Midi), NoteType)}")
                .ToString();
        }
    }
}
