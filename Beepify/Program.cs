using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Beepify.MIDI;

namespace Beepify
{
    class Program
    {
        static void Main(string[] args)
        {
            Midi mid = new Midi("smb.mid");
            Beep(1000, 100);
            Console.ReadKey();
        }

        static void PlayTone(int fqr, int dur)
        {
            ThreadStart childref = new ThreadStart(() => Beep(fqr, dur));
            Thread childThread = new Thread(childref);
        }

        static void Beep(int frq, int dur)
        {
            Console.Beep(frq, dur);
        }
    }
}
