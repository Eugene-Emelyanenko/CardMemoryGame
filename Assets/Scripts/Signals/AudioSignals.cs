namespace Signals
{
    public static class AudioSignals
    {
        public sealed class PlayMusic 
        { 
            public string Id; public float Fade; public bool Loop;

            public PlayMusic(string id, float fade = 0.5f, bool loop = true)
            {
                Id=id; Fade=fade; Loop=loop; 
            } 
        }

        public sealed class StopMusic
        {
            public float Fade;

            public StopMusic(float fade = 0.5f)
            {
                Fade=fade; 
                
            }
        }

        public sealed class PlaySfx
        { 
            public string Id; 
            public float Volume;

            public PlaySfx(string id, float volume = 1f)
            {
                Id=id; Volume=volume; 
            } 
        }

        public sealed class SetMusicVolume { public float Value; public SetMusicVolume(float v){ Value=v; } }
        public sealed class SetSfxVolume   { public float Value; public SetSfxVolume(float v){ Value=v; } }
        public sealed class SetMasterVolume   { public float Value; public SetMasterVolume(float v){ Value=v; } }
    }
}