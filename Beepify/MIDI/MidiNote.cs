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

        public MidiNote(byte note, byte velocity, byte channel, Events.Midi noteType)
        {
            Note = note;
            Velocity = velocity;
            Channel = channel;
            NoteType = noteType;
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append($"Note: {Note.ToString("X")}, ")
                .Append($"Velocity: {Velocity.ToString("X")}, ")
                .Append($"Channel: {Channel.ToString("X")}, ")
                .Append($"Node Type: {Enum.GetName(typeof(Events.Midi), NoteType)}")
                .ToString();
        }
    }
}
