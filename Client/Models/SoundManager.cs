using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;

namespace Client.Models
{
    public class SoundManager
    {
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        public void PlaySound(string filePath)
        {
            this.soundPlayer.SoundLocation = filePath;
            this.soundPlayer.Play();
        }

        public void PlayWinSound()
        {
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            filePath += "\\Resources\\winSound.wav";
            this.soundPlayer.SoundLocation = filePath;
            this.soundPlayer.Play();
        }

        public void PlayLoseSound()
        {
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            filePath += "\\Resources\\loseSound.wav";
            this.soundPlayer.SoundLocation = filePath;
            this.soundPlayer.Play();
        }
    }
}
