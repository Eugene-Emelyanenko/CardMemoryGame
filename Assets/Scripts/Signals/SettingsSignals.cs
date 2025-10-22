namespace Signals
{
    public static class SettingsSignals
    {
        public sealed class MusicChanged { public float Value; public MusicChanged(float v){ Value = v; } }
        public sealed class SfxChanged   { public float Value; public SfxChanged(float v){ Value = v; } }
        public sealed class MasterChanged   { public float Value; public MasterChanged(float v){ Value = v; } }
        public sealed class BackClicked  { }
    }
}