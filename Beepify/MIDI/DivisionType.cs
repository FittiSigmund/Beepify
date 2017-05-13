using System;

namespace Beepify.MIDI
{
    public struct DivisionType
    {
        public bool SignBit { get; private set; }
        public ushort TicksPerQuarter { get; private set; }
        public byte FramesPerSecond { get; private set; }
        public byte TicksPerFrame { get; private set; }
        public bool DropFrame { get; private set; }

        // Division structure:
        //
        // +-----------+----+----------------+-------------+
        // | Bit:      | 15 |     14...8     |    7...0    |
        // +-----------+----+----------------+-------------+
        // | Division: |  0 |    ticks per quarter note    |
        // | Division: |  1 | -frames/second | ticks/frame |
        // +-----------+----+----------------+-------------+

        public DivisionType(ushort input)
        {
            // Remove signbit
            ushort withoutSign = (ushort)((ushort)(input << 1) >> 1);

            // Get sign bit
            SignBit = Convert.ToBoolean(input >> 15);

            // This is active if signbit isn't
            TicksPerQuarter = withoutSign;

            // 7 bytes after signbit
            FramesPerSecond = (byte)(withoutSign >> 8);

            // 8 last bytes
            TicksPerFrame = (byte)(withoutSign);

            // Active if TicksPerFrame is negative
            DropFrame = (TicksPerFrame >= 128);
        }
    }
}
