namespace Game.Audio
{
    public static class AudioKeys
    {
        // PlayerPrefs keys
        public const string PlayerPrefMusicVolume = "MusicVolume";
        public const string PlayerPrefMasterVolume = "MasterVolume";
        public const string PlayerPrefSfxVolume = "SfxVolume";
        public const string PlayerPrefToggleMute = "ToggleMute";

        // AudioMixer parameter names (musi odpowiadaæ nazwom w AudioMixerze)
        public const string MixerMusicParam = "MusicVolume";
        public const string MixerMasterParam = "MasterVolume";
        public const string MixerSfxParam = "SfxVolume";
    }
}