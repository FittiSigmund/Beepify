using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Beepify.MIDI.Events;
using static Beepify.MIDI.Midi;

namespace Beepify.MIDI
{
    public class MidiEvent
    {
        public EventTypes EventType { get; private set; }
        public Meta MetaType { get; private set; }
        public ControllerType ControllerType { get; private set; }
        public Events.Midi MidiType { get; private set; }
        public uint Size { get; private set; }
        public byte[] EventData { get; private set; }

        // A length of -1 means variable
        public static Dictionary<byte, int> MetaLengths = new Dictionary<byte, int>() {
            { (byte) Meta.SEQUENCE_NUM, 3 },
            { (byte) Meta.TEXT_EVNT, -1 },
            { (byte) Meta.COPYRIGHT_NOTICE, -1 },
            { (byte) Meta.SEQUENCE_NAME, -1 },
            { (byte) Meta.INSTRUMENT_NAME, -1 },
            { (byte) Meta.LYRICS, -1 },
            { (byte) Meta.MARKER, -1 },
            { (byte) Meta.CHANNEL_PREFIX, 2 },
            { (byte) Meta.END_TRACK, 1 },
            { (byte) Meta.TEMPO_CHANGE, 4 },
            { (byte) Meta.SMPTE_OFFSET, 6 },
            { (byte) Meta.TIME_SIGNATURE, 5 },
            { (byte) Meta.KEY_SIGNATURE, 3 },
            { (byte) Meta.SEQUENCER_SPECIFIC, -1 },
        };

        public MidiEvent(byte[] data)
        {


            switch (data[0])
            {
                case 0xFF:
                    EventType = EventTypes.Meta;
                    MetaType = (Meta)data[1];
                    int tempSize = MetaLengths[(byte)MetaType];
                    Size = tempSize == -1 ? VariableLength(data.Skip(2).ToArray()) : (uint) tempSize;
                    break;
                case 0xF0:
                case 0xF7:
                    EventType = EventTypes.Sysex;
                    Size = VariableLength(data.Skip(1).ToArray());
                    break;
                default:
                    EventType = EventTypes.Midi;
                    MidiType = (Events.Midi) data[1];
                    break;
            }

        }
    }
}
