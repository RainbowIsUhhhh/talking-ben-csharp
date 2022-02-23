using System;
using System.Threading;

namespace Talking_Ben_CSharp
{
    class Program
    {
        public static Random rnd = new Random();

        public static System.Media.SoundPlayer ben = new System.Media.SoundPlayer(@"Audio/ben.wav");

        public static bool talking = false;
        public static bool hasTalked = false;

        private static void timer1(Object state)
        {
            if (talking == false)
            {
                if (hasTalked == true)
                {
                    int speech = rnd.Next(1, 5);

                    switch (speech)
                    {
                        case 1:
                            ben.SoundLocation = @"Audio/ben_yes.wav";
                            Console.WriteLine("Ben says: Yes");
                            break;
                        case 2:
                            ben.SoundLocation = @"Audio/ben_no.wav";
                            Console.WriteLine("Ben says: No");
                            break;
                        case 3:
                            ben.SoundLocation = @"Audio/ben_laugh.wav";
                            Console.WriteLine("Ben laughs");
                            break;
                        case 4:
                            ben.SoundLocation = @"Audio/ben_taunt.wav";
                            Console.WriteLine("Ben taunts");
                            break;
                    }

                    ben.Play();

                    hasTalked = false;
                }
            }
        }

        private static void ShowPeakMono(object sender, NAudio.Wave.WaveInEventArgs args)
        {
            float maxValue = 32767;
            int peakValue = 0;
            int bytesPerSample = 2;
            for (int index = 0; index < args.BytesRecorded; index += bytesPerSample)
            {
                int value = BitConverter.ToInt16(args.Buffer, index);
                peakValue = Math.Max(peakValue, value);
            }

            if ((peakValue / maxValue) > 0.065)
            {
                talking = true;
                hasTalked = true;
            }
            else
            {
                talking = false;
            }
        }

        static void Main(string[] args)
        {
            ben.Play();
            Thread.Sleep(1000);
            Timer t = new Timer(timer1, 5, 0, 2000);
            var waveIn = new NAudio.Wave.WaveInEvent
            {
                DeviceNumber = 0, // customize this to select your microphone device
                WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 2),
                BufferMilliseconds = 50
            };
            waveIn.DataAvailable += ShowPeakMono;
            waveIn.StartRecording();
            while (true) { }
        }
    }
}
