using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public sealed class CardView : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Button button;

        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Animation")] [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private float matchedFade = 0.2f;

        public int Index { get; private set; }
        public string Id { get; private set; }
        public Sprite Face { get; private set; }
        public Sprite Back { get; private set; }
        public bool IsFront { get; private set; } = true;

        private Tween _tween;
        private UnityAction<int> _onClick;

        private void Awake()
        {
            canvasGroup.alpha = 1f;
            button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(HandleClick);
            _tween?.Kill();
        }

        public void Setup(int index, string id, Sprite face, Sprite back, UnityAction<int> onClick)
        {
            Index = index;
            Id = id;
            Face = face;
            Back = back;
            _onClick = onClick;
            SetFrontImmediate(true);
        }

        private void HandleClick()
        {
            _onClick?.Invoke(Index);
        }

        public void SetFrontImmediate(bool front)
        {
            _tween?.Kill();
            IsFront = front;
            image.rectTransform.localScale = Vector3.one;
            image.sprite = front ? Face : Back;
            canvasGroup.alpha = 1f;
            button.interactable = true;
        }

        public void FlipToFront()
        {
            if (!IsFront) Flip(true);
        }

        public void FlipToBack()
        {
            if (IsFront) Flip(false);
        }

        public Tween FlipToFrontTween() => IsFront ? null : Flip(true);
        public Tween FlipToBackTween() => !IsFront ? null : Flip(false);

        public void HideAfterMatch(float delaySeconds = 1f)
        {
            _tween?.Kill();
            button.interactable = false;

            _tween = DOVirtual.DelayedCall(delaySeconds, () => { })
                .OnComplete(() =>
                {
                    canvasGroup
                        .DOFade(0f, matchedFade)
                        .SetUpdate(true)
                        .SetLink(gameObject);
                })
                .SetUpdate(true)
                .SetLink(gameObject);
        }

        private Tween Flip(bool toFront)
        {
            _tween?.Kill();
            button.interactable = false;

            var rt = image.rectTransform;

            _tween = DOTween.Sequence()
                .Append(rt.DOScaleX(0f, flipDuration * 0.5f).SetEase(Ease.OutQuad))
                .AppendCallback(() =>
                {
                    IsFront = toFront;
                    image.sprite = toFront ? Face : Back;
                })
                .Append(rt.DOScaleX(1f, flipDuration * 0.5f).SetEase(Ease.OutQuad))
                .OnComplete(() => button.interactable = true)
                .SetUpdate(true)
                .SetLink(gameObject);

            return _tween;
        }
    }
}