using UnityEngine;
using UnityEngine.UI;
using Base;
using DG.Tweening;
using TMPro;
using Signals;

namespace Views
{
    public class PauseView : View
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button menuButton;
        
        public override void Bind()
        {
            base.Bind();
            
            pauseButton.onClick.AddListener(OnPause);
            continueButton.onClick.AddListener(OnContinue);
            menuButton.onClick.AddListener(OnMenu);
        }

        public override void Unbind()
        {
            pauseButton.onClick.RemoveListener(OnPause);
            continueButton.onClick.RemoveListener(OnContinue);
            menuButton.onClick.RemoveListener(OnMenu);
            
            base.Unbind();
        }
        
        private void OnPause() => Bus.Fire(new PauseSignals.PauseClicked());
        private void OnContinue() => Bus.Fire(new PauseSignals.ContinueClicked());
        private void OnMenu() => Bus.Fire(new PauseSignals.MenuClicked());
    }
}