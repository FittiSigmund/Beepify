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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">Path to midi file</param>
        public Midi(string path)
        {
            // Read entire file
            byte[] file = File.ReadAllBytes(path);

            // Get file header
            Header = new HeaderChunk(file);

            // Parse remaining chunks
            Chunks = ParseMTrKChunks(file, 14);
        }

        /// <summary>
        /// Parses MTrK chunks and returns an array of
        /// chunks
        /// </summary>
        /// <param name="bytes">Where to parse from</param>
        /// <param name="offset">Where to start parsing</param>
        /// <returns>Returns an array of chunks</returns>
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

        /// <summary>
        /// Overload method
        /// </summary>
        /// <param name="data">Where to find VLV</param>
        /// <returns>Length</returns>
        public static uint VariableLength(byte[] data)
        {
            int byteLength;
            return VariableLength(data, out byteLength);
        }

        /// <summary>
        /// The MIDI format uses a format called VLV which is the length
        /// of something given in a variable bytelength. As long as the 
        /// sign bit is set, the latter bits are added to an integer
        /// such that foreach byte there will be added 7 bits
        /// </summary>
        /// <param name="data">Where to find VLV</param>
        /// <param name="byteLength">Bytelength of VLV</param>
        /// <returns>Length</returns>
        public static uint VariableLength(byte[] data, out int byteLength)
        {
            uint length = 0;
            int i = 0;
            do
            {
                byte toAdd = data[i];
                length |= (uint)((toAdd & ~(1 << 7)) << (i * 7));
            } while (data[i++] >= 0x80);

            byteLength = i;
            return length;
        }

        /// <summary>
        /// Gets defined amount of bytes from an array of bytes
        /// with defined start index and whether it should be
        /// little or big endian
        /// </summary>
        /// <param name="bytes">Where to get bytes from</param>
        /// <param name="index">Where to start getting</param>
        /// <param name="amount">Amount of bytes to get</param>
        /// <param name="msb">Most significant fist</param>
        /// <returns>Array of bytes</returns>
        public static byte[] GetBytes(byte[] bytes, int index, int amount, bool msb = false)
        {
            byte[] returnBytes = bytes.Skip(index).Take(amount).ToArray();
            // Reverse if msb
            return msb ? returnBytes.Reverse().ToArray() : returnBytes;
        }

        /// <summary>
        /// Parse enum string value or throw error
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to be parsed</param>
        /// <returns>Actual enum value</returns>
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
