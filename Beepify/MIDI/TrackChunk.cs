using System;
using System.Collections.Generic;
using System.Linq;
using static Beepify.MIDI.Midi;

namespace Beepify.MIDI
{
    public class TrackChunk
    {
        public ChunkTypes ChunkType { get; private set; }
        public uint ChunkSize { get; private set; }
        public byte[] ChunkData { get; private set; }
        public MidiEvent[] Events { get; private set; }
        public uint DeltaTime { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="file">Array of bytes from where
        /// chunk will be created</param>
        public TrackChunk(byte[] file)
        {
            int pntr = 0;

            // Get size of chunk
            ChunkSize = BitConverter.ToUInt32(GetBytes(file, 4, 4, true), 0);

            // We already know this is a MTrK chunk
            ChunkType = ChunkTypes.MTrk;

            // Get all the chunk data
            ChunkData = file.Skip(8).Take((int) ChunkSize).ToArray();

            // Wierd delta time
            DeltaTime = VariableLength(ChunkData, out pntr);
            pntr--;
            // Parse all events in chunk
            List<MidiEvent> eventList = new List<MidiEvent>();
            while(pntr < ChunkSize - 1)
            {
                eventList.Add(new MidiEvent(ChunkData, ref pntr));
            }
            Events = eventList.ToArray();
        }
    }
}
