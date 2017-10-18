using System;
using System.Collections.Generic;
using System.Linq;
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
        public byte Channel { get; private set; }
        public uint Size { get; private set; }
        public byte[] EventData { get; private set; }
        public MidiNote Note { get; private set; }
        public int Ticks { get; private set; }

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
            { (byte) 0x21, 3 },
        };

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="data">Where to get event</param>
        /// <param name="pntr">Where to start getting</param>
        public MidiEvent(byte[] data, ref int pntr)
        {
            // Used whenever we have variable lengths
            int tempPntr = 0;
            Ticks = (int) VariableLength(data.Skip(pntr).ToArray(), out tempPntr);
            pntr += tempPntr;
            tempPntr = 0;
            // What is this event
            switch (data[pntr++])
            {
                // Meta
                case 0xFF:
                    EventType = EventTypes.Meta;
                    MetaType = (Meta)data[pntr++];
                    int tempSize = MetaLengths[(byte)MetaType];
                    Size = tempSize == -1 ? VariableLength(data.Skip(pntr).ToArray(), out tempPntr) : (uint)tempSize;
                    pntr += tempPntr;
                    EventData = data.Skip(pntr).Take((int)Size).ToArray();
                    break;
                // Sysex
                case 0xF0:
                case 0xF7:
                    EventType = EventTypes.Sysex;
                    Size = VariableLength(data.Skip(pntr).ToArray(), out tempPntr);
                    pntr += tempPntr;
                    EventData = data.Skip(pntr).Take((int)Size).ToArray();
                    break;
                // Midi or controller
                default:
                    int msb = data[pntr - 1] >> 4;
                    EventType = EventTypes.Midi;

                    if(msb == 0x0C || msb == 0x0D)
                    {
                        Size = 1;
                    }
                    else if(msb == 0x0A || msb == 0x0E || msb == 0x0B)
                    {
                        Size = 2;
                    }
                    //Midi
                    else if (Enum.IsDefined(typeof(Events.Midi), msb))
                    {
                        // The first 4 bits determine MIDI type
                        MidiType = (Events.Midi) (data[pntr - 1] >> 4);
                        // Last 4 bits select the channel
                        Channel = (byte)((byte)(data[pntr - 1] << 4) >> 4);
                        // It is always 2
                        Size = 2;
                        // Get the latter 2 bytes
                        EventData = data.Skip(pntr).Take((int) Size).ToArray();
                        // Create note
                        Note = new MidiNote(EventData[0], EventData[1], Channel, Ticks, MidiType);
                    }
                    else
                    {
                        // unknown type :p
                        throw new NotImplementedException($"{data[pntr - 1].ToString("X")} is not defined");
                    }
                    break;
            }

            // Set pointer to after EventData
            pntr += (int) Size;
        }
    }
}
