using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Beepify.MIDI
{
    public class Midi
    {
        public enum ChunkTypes
        {
            MThd,
            MTrk
        }

        public enum EventTypes
        {
            Midi,
            Sysex,
            Meta
        }
        
        public HeaderChunk Header { get; private set; }
        public TrackChunk[] Chunks { get; private set; }

        public Midi(string path)
        {
            // Read entire file
            byte[] file = File.ReadAllBytes(path);

            // Get file header
            Header = new HeaderChunk(file);

            // Parse remaining chunks
            Chunks = ParseMTrKChunks(file, 14);
        }

        private static TrackChunk[] ParseMTrKChunks(byte[] bytes, int offset)
        {
            List<TrackChunk> chunks = new List<TrackChunk>();
            string type;

            while (offset < bytes.Length && (type = Encoding.Default.GetString(bytes.Skip(offset).Take(4).ToArray())) == "MTrk")
            {
                TrackChunk chunk = new TrackChunk(bytes.Skip(offset).ToArray());
                offset += (int)(chunk.ChunkSize + 8);
                chunks.Add(chunk);
            }

            return chunks.ToArray();
        }

        static string Pad(uint b, int length = 32)
        {
            return Convert.ToString(b, 2).PadLeft(length, '0');
        }

        public static uint VariableLength(byte[] data)
        {
            uint length = 0;
            int i = 0;
            do
            {
                byte toAdd = data[i];
                length |= (uint)((toAdd & ~(1 << 7)) << (i * 7));
            } while (data[i++] >= 0x80);
            return length;
        }

        public static byte[] GetBytes(byte[] bytes, int index, int amount, bool msb = false)
        {
            byte[] returnBytes = bytes.Skip(index).Take(amount).ToArray();
            return msb ? returnBytes.Reverse().ToArray() : returnBytes;
        }

        public static T ParseEnum<T>(string value)
        {
            // Parse enum type
            T enumVal = (T)Enum.Parse(typeof(T), value, ignoreCase: false);
            // Check if type is defined
            if (Enum.IsDefined(typeof(T), enumVal))
            {
                return enumVal;
            }
            else
            {
                // Type was not defined
                throw new InvalidDataException("Could not parse enum");
            }
        }
    }
}
