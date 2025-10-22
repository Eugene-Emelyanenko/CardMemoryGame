using UnityEngine;
using Zenject;
using DG.Tweening;

namespace Base
{
    public abstract class View : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] protected CanvasGroup canvasGroup;

        private Tween fadeTween;
        
        protected bool _isBound;
        protected SignalBus Bus { get; private set; }
        
        [Inject]
        private void InjectBus(SignalBus bus) => Bus = bus;

        public virtual void Bind()
        {
            if (_isBound) return;
            _isBound = true;
        }

        public virtual void Unbind()
        {
            if (!_isBound) return;
            _isBound = false;
        }

        public void Show(float duration = 1f)
        {
            fadeTween?.Kill();
            canvasGroup.alpha = 0f;
            
            gameObject.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            fadeTween = canvasGroup
                .DOFade(1f, duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(gameObject);
        }

        public void Hide(float duration = 1f)
        {
            fadeTween?.Kill();
            
            fadeTween = canvasGroup
                .DOFade(0f, duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    gameObject.SetActive(false);
                })
                .SetLink(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_isBound) Unbind();
        }
    }
}