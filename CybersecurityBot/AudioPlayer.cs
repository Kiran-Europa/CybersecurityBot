using System;
using System.Diagnostics;
using System.IO;

namespace CybersecurityBot
{
    static class AudioPlayer
    {
        private const string WavFileName = "greeting.wav";

        public static void PlayGreeting()
        {
            string wavPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, WavFileName);

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
                // Use PowerShell to play WAV — works on Windows with .NET 10
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-c (New-Object Media.SoundPlayer '{wavPath}').PlaySync()",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                var proc = Process.Start(psi);
                proc?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[Audio] Could not play greeting: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}