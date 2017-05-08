using System;
using System.IO;
using System.Linq;
using System.Text;
using static Beepify.MIDI.Midi;

namespace Beepify.MIDI
{
    public struct HeaderChunk
    {
        public ChunkTypes ChunkType { get; private set; }
        public uint ChunkSize { get; private set; }
        public ushort Format { get; private set; }
        public ushort Tracks { get; private set; }
        public DivisionType Division { get; private set; }

        public HeaderChunk(byte[] file)
        {
            // Chunk type as string (first 4 bytes)
            string type = Encoding.Default.GetString(file.Take(4).ToArray());

            // Test chunk type
            try
            {
                ChunkType = ParseEnum<ChunkTypes>(type);
            }
            catch (InvalidDataException)
            {
                throw new InvalidDataException("Could not parse MIDI chunk type");
            }

            // Load length MSB
            ChunkSize = BitConverter.ToUInt32(GetBytes(file, 4, 4, true), 0);
            Format = BitConverter.ToUInt16(GetBytes(file, 8, 2, true), 0);
            Tracks = BitConverter.ToUInt16(GetBytes(file, 10, 2, true), 0);
            Division = new DivisionType(BitConverter.ToUInt16(GetBytes(file, 12, 2, true), 0));
        }
    }
}
