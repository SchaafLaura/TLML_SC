using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadConsoleGame.Audio
{
    internal class NullAudioProvider : AudioProvider
    {
        public override int AddSound(string path) => 0;
        public override void PlaySound(int soundID) { }
        public override void StopSound(int soundID) { }
        public override void PauseSound(int soundID) { }
        public override void StopAllSounds() { }
    }
    internal abstract class AudioProvider
    {
        public abstract int AddSound(string path);
        public abstract void PlaySound(int soundID);
        public abstract void StopSound(int soundID);
        public abstract void PauseSound(int soundID);
        public abstract void StopAllSounds();
    }
}
