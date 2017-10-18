using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beepify.MIDI;
using System.Threading;
using System.IO;
using System.Media;

namespace Beepify.MIDIPlayer
{
    class MidiPlayer
    {
        private Midi mid;
        private List<PlayerNote[]> Tracks;
        private int[] channels = new int[16];
        private int lastTemp = 50000;
        private int index = 0;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mid">Midi instance</param>
        public MidiPlayer(Midi mid)
        {
            this.mid = mid;

            Tracks = new List<PlayerNote[]>();




            for (int c = 0; c < mid.Chunks.Length; c++)
            {
                TrackChunk chunk = mid.Chunks[c];
                List<PlayerNote> track = new List<PlayerNote>();
                for (int me = 0; me < chunk.Events.Length; me++)
                {
                    MidiEvent e = chunk.Events[me];

                    if(e.EventType == Midi.EventTypes.Meta)
                    {
                        string toPrint = "";
                        switch (e.MetaType)
                        {
                            case Events.Meta.CUE_POINT:
                            case Events.Meta.CHANNEL_PREFIX:
                                break;
                            case Events.Meta.SEQUENCE_NAME:
                            case Events.Meta.COPYRIGHT_NOTICE:
                            case Events.Meta.INSTRUMENT_NAME:
                            case Events.Meta.TEXT_EVNT:
                                toPrint = Encoding.Default.GetString(e.EventData);
                                break;
                            case Events.Meta.END_TRACK:
                                toPrint = $"Track ended, nodes: {track.Count}";
                                Tracks.Add(track.ToArray());
                                track = new List<PlayerNote>();
                                break;
                            case Events.Meta.TEMPO_CHANGE:
                                lastTemp = BitConverter.ToInt32(e.EventData, 0);
                                toPrint = lastTemp.ToString();
                                break;
                            case Events.Meta.TIME_SIGNATURE:
                                toPrint = string.Format("nn: {0}, dd: {1}, cc: {2}, bb: {3}",
                                    e.EventData[1],
                                    e.EventData[2],
                                    e.EventData[3],
                                    e.EventData[4]
                                );
                                break;
                        }
                        Console.WriteLine($"{e.MetaType}: {toPrint}");
                        continue;
                    }

                    if (e.EventType == Midi.EventTypes.Midi && (e.MidiType == Events.Midi.MIDI_EVENT_NOTE_ON ||
                        e.MidiType == Events.Midi.MIDI_EVENT_NOTE_OFF))
                    {
                        if(e.Ticks > 0x20)
                        {
                            for (int i = 0; i < mid.Header.Division.TicksPerQuarter / e.Ticks; i++)
                            {
                                track.Add(null);
                            }
                        }

                        if (e.MidiType == Events.Midi.MIDI_EVENT_NOTE_ON)
                        {
                            PlayerNote note = new PlayerNote(
                                e.Note.Note, 
                                0,
                                1, 
                                lastTemp, 
                                (ushort)((128.0 / e.Note.Velocity)*ushort.MaxValue)
                            );

                            channels[e.Note.Channel] = me;

                            track.Add(note);
                        }

                        else if (e.MidiType == Events.Midi.MIDI_EVENT_NOTE_OFF)
                        {
                            channels[e.Note.Channel] = 0;
                        }
                    }
                }
            }

            while (PlayQuarter())
            {
                Thread.Sleep(240);
            }
            Console.WriteLine("Done");
        }

        private bool PlayQuarter()
        {
            bool ret = false;
            List<Action> play = new List<Action>();
            foreach (PlayerNote[] track in Tracks.Take(2))
            {
                if(track.Length > index)
                {
                    PlayerNote note = track[index];
                    if(note != null)
                    {
                        play.Add(() => PlayBeep((ushort)note.Frequency, note.Duration, note.Volume));
                        //Console.WriteLine(note);
                    }else
                    {
                        //Console.WriteLine("Skip note");
                    }

                    ret = true;
                }
            }

            index++;

            Parallel.Invoke(play.ToArray());

            return ret;
        }


        /// <summary>
        /// Plays the midi tracks
        /// </summary>
        public void Play()
        {
            // Mary, just delete when implementation starts
            PlayBeep(2470, 400);
            PlayBeep(2200, 400);
            PlayBeep(1960, 400);
            PlayBeep(2200, 400);
            PlayBeep(2470, 400);
            PlayBeep(2470, 400);
            PlayBeep(2470, 800);

            PlayBeep(2200, 400);
            PlayBeep(2200, 400);
            PlayBeep(2200, 800);

            PlayBeep(2470, 400);
            PlayBeep(2940, 400);
            PlayBeep(2940, 800);

            PlayBeep(2470, 400);
            PlayBeep(2200, 400);
            PlayBeep(1960, 400);
            PlayBeep(2200, 400);
            PlayBeep(2470, 400);
            PlayBeep(2470, 400);
            PlayBeep(2470, 800);

            PlayBeep(2470, 400);
            PlayBeep(2200, 400);
            PlayBeep(2200, 400);
            PlayBeep(2470, 400);
            PlayBeep(2200, 400);
            PlayBeep(3920, 400);
            // Tada
        }


        //https://stackoverflow.com/questions/12611982/generate-audio-tone-to-sound-card-in-c-or-c-sharp
        public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++)
                {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }

            mStrm.Seek(0, SeekOrigin.Begin);
            new SoundPlayer(mStrm).Play();
            writer.Close();
            mStrm.Close();
        } // public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
    }


}
