using UnityEngine;
using UnityEngine.UI;
using Base;
using TMPro;
using Signals;

namespace Views
{
    public class SettingsView : View
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI musicValueText;
        [SerializeField] private TextMeshProUGUI sfxValueText;

        public override void Bind()
        {
            base.Bind();
            musicSlider.onValueChanged.AddListener(HandleMusic);
            sfxSlider.onValueChanged.AddListener(HandleSfx);
            backButton.onClick.AddListener(HandleBack);
        }

        public override void Unbind()
        {
            musicSlider.onValueChanged.RemoveListener(HandleMusic);
            sfxSlider.onValueChanged.RemoveListener(HandleSfx);
            backButton.onClick.RemoveListener(HandleBack);
            base.Unbind();
        }

        public void SetMusicValue(float v)
        {
            musicSlider.SetValueWithoutNotify(v);
            musicValueText.text = Mathf.RoundToInt(v * 100) + "%";
        }

        public void SetSfxValue(float v)
        {
            sfxSlider.SetValueWithoutNotify(v);
            sfxValueText.text = Mathf.RoundToInt(v * 100) + "%";
        }

        private void HandleMusic(float v)
        {
            Bus.Fire(new SettingsSignals.MusicChanged(v));
            SetMusicValue(v);
        }

        private void HandleSfx(float v)
        {
            Bus.Fire(new SettingsSignals.SfxChanged(v));
            SetSfxValue(v);
        }

        private void HandleBack() => Bus.Fire(new SettingsSignals.BackClicked());
    }
}
