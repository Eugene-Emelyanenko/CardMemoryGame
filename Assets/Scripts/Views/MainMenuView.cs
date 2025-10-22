using UnityEngine;
using UnityEngine.UI;
using Base;
using Signals;

namespace Views
{
    public class MainMenuView : View
    {
        [Header("UI Elements")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;

        public override void Bind()
        {
            base.Bind();
            playButton.onClick.AddListener(OnPlay);
            settingsButton.onClick.AddListener(OnSettings);
        }

        public override void Unbind()
        {
            playButton.onClick.RemoveListener(OnPlay);
            settingsButton.onClick.RemoveListener(OnSettings);
            base.Unbind();
        }

        private void OnPlay() => Bus.Fire(new MainMenuSignals.PlayClicked());
        private void OnSettings() => Bus.Fire(new MainMenuSignals.SettingsClicked());
    }
}