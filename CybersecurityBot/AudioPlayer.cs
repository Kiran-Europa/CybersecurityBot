using System;
using System.Diagnostics;
using System.IO;

namespace CybersecurityBot
{
    static class AudioPlayer
    {
        // the name of the wav file we want to play
        private const string WavFileName = "greeting.wav";

        public static void PlayGreeting()
        {
            // builds the full path to where the wav file should be
            string wavPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, WavFileName);

            // if the file doesnt exist, print a message and stop
            if (!File.Exists(wavPath))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[Audio] '{WavFileName}' not found — skipping voice greeting.");
                Console.WriteLine($"        Place your WAV file here: {wavPath}");
                Console.ResetColor();
                return;
            }

            try
            {
                // uses powershell to play the wav file
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-c (New-Object Media.SoundPlayer '{wavPath}').PlaySync()",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                var proc = Process.Start(psi);
                // waits for the audio to finish before continuing
                proc?.WaitForExit();
            }
            catch (Exception ex)
            {
                // if something goes wrong, show the error in red
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[Audio] Could not play greeting: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}