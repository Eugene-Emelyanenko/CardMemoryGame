using UnityEngine;
using UnityEngine.UI;
using Base;
using TMPro;
using Signals;

namespace Views
{
    public class SettingsView : View
    {
        [Header("Music")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private TextMeshProUGUI musicValueText;
        
        [Header("Sfx")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TextMeshProUGUI sfxValueText;
        
        [Header("Master")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private TextMeshProUGUI masterValueText;
        
        [Header("Other")]
        [SerializeField] private Button backButton;

        public override void Bind()
        {
            base.Bind();
            
            musicSlider.onValueChanged.AddListener(HandleMusic);
            sfxSlider.onValueChanged.AddListener(HandleSfx);
            masterSlider.onValueChanged.AddListener(HandleMaster);
            
            backButton.onClick.AddListener(HandleBack);
        }

        public override void Unbind()
        {
            musicSlider.onValueChanged.RemoveListener(HandleMusic);
            sfxSlider.onValueChanged.RemoveListener(HandleSfx);
            masterSlider.onValueChanged.RemoveListener(HandleMaster);
            
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
        
        public void SetMasterValue(float v)
        {
            masterSlider.SetValueWithoutNotify(v);
            masterValueText.text = Mathf.RoundToInt(v * 100) + "%";
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
        
        private void HandleMaster(float v)
        {
            Bus.Fire(new SettingsSignals.MasterChanged(v));
            SetMasterValue(v);
        }

        private void HandleBack() => Bus.Fire(new SettingsSignals.BackClicked());
    }
}
