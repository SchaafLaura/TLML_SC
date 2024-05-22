namespace SadConsoleGame.Audio
{
    internal class IngameAudioProvider : AudioProvider
    {
        public int playerDamageSound;
        public int playerPickupScrapSound;
        public int playerPickupWeaponSound;
        public int playerPickupArmorSound;
        public int playerPickupMagicSound;

        private List<CachedSound> sounds;
        public IngameAudioProvider() 
        {
            sounds = new();
            for(int i = 0; i < 62; i++)
                AddSound(i.ToString() + ".wav");
        }
        public override int AddSound(string path)
        {
            /* var soundFilePath = Util.ConcatWithSystemPathSeparator(
                 ".",
                 "Assets",
                 "Audio",
                 path
                 );*/
            var soundFilePath = ".\\Audio\\sounds\\" + path;
            var sound = new CachedSound(soundFilePath);

            sounds.Add(sound);
            return sounds.Count - 1;
        }
        public override void PlaySound(int soundID) 
        {
            AudioPlaybackEngine.Instance.PlaySound(sounds[soundID]);
        }
        public override void StopSound(int soundID) { }
        public override void PauseSound(int soundID) { }
        public override void StopAllSounds() { }
    }
}
