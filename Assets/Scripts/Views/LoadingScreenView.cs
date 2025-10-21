using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Base;

namespace Views
{
    public class LoadingScreenView : View
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image loadingBar;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Animation")]
        [SerializeField] private float fadeDuration = 0.35f;

        private Tween fadeTween;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        public void SetProgress(float v)
        {
            loadingBar.fillAmount = v;
            loadingText.text = $"{Mathf.RoundToInt(v * 100)}%";
        }

        public override void Show()
        {
            fadeTween?.Kill();
            canvasGroup.alpha = 0f;
            
            base.Show();

            fadeTween = canvasGroup
                .DOFade(1f, fadeDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(gameObject);
        }

        public override void Hide()
        {
            fadeTween?.Kill();
            
            fadeTween = canvasGroup
                .DOFade(0f, fadeDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    base.Hide();
                })
                .SetLink(gameObject);
        }
    }
}
