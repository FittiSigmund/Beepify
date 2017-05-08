using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TrackChunk(byte[] file)
        {
            ChunkSize = BitConverter.ToUInt32(GetBytes(file, 4, 4, true), 0);
            ChunkType = ChunkTypes.MTrk;
            ChunkData = file.Skip(8).Take((int)ChunkSize).ToArray();
            DeltaTime = VariableLength(ChunkData);
        }
    }
}
