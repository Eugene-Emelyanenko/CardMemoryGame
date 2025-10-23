using UnityEngine;
using Zenject;

namespace Global
{
    public class FpsConfig : IInitializable
    {
        private readonly int _targetFps;
    
        public FpsConfig(int targetFps = 60)
        {
            _targetFps = targetFps;
        }
    
        public void Initialize()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFps;
        }
    }
}