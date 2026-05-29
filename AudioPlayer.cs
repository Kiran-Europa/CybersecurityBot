using System;
using System.Diagnostics;
using System.IO;

namespace CybersecurityBotGUI
{
    /// <summary>
    /// Plays a greeting WAV file using PowerShell (no extra dependencies).
    /// </summary>
    static class AudioPlayer
    {
        private const string WavFileName = "greeting.wav";

        /// <summary>
        /// Plays the sound if the file exists. Fails silently on error.
        /// </summary>
        public static void PlayGreeting()
        {
            string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WavFileName);
            if (!File.Exists(wavPath)) return;

            try
            {
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
            catch { /* Ignore audio errors */ }
        }
    }
}