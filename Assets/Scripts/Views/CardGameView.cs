using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Views
{
    public class CardGameView : Base.View
    {
        [Header("Scene Refs")]
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private Transform gameField;
        [SerializeField] private TextMeshProUGUI correctGuessesText;

        private readonly List<CardView> _cards = new();
        public int Count => _cards.Count;

        public void ClearField()
        {
            foreach (Transform t in gameField)
                Destroy(t.gameObject);
            _cards.Clear();
        }

        public void BuildSlots(int total, IReadOnlyList<string> ids, IReadOnlyList<Sprite> faces, Sprite back)
        {
            ClearField();

            for (int i = 0; i < total; i++)
            {
                CardView card = Instantiate(cardPrefab, gameField);
                card.Setup(i, ids[i], faces[i], back, OnCardClicked);
                _cards.Add(card);
            }
        }

        private void OnCardClicked(int index)
        {
            Bus.Fire(new Signals.CardGameSignals.CardClicked(index));
        }

        public void SetCounter(int matched, int totalPairs)
        {
            if (correctGuessesText)
                correctGuessesText.text = $"{matched}/{totalPairs}";
        }

        public bool   IsFront (int index) => _cards[index].IsFront;
        public bool   IsActive(int index) => _cards[index].gameObject.activeSelf;
        public string GetId  (int index)  => _cards[index].Id;

        public void RevealAllImmediate() { foreach (var c in _cards) c.SetFrontImmediate(true); }
        public void ConcealAllImmediate(){ foreach (var c in _cards) c.SetFrontImmediate(false); }

        public void FlipToFront(int index) => _cards[index].FlipToFront();
        public void FlipToBack (int index) => _cards[index].FlipToBack();
        
        public DG.Tweening.Tween FlipToFrontTween(int index) => _cards[index].FlipToFrontTween();
        public DG.Tweening.Tween FlipToBackTween (int index) => _cards[index].FlipToBackTween();

        public void HideMatched(int a, int b, float delaySec)
        {
            _cards[a].HideAfterMatch(delaySec);
            _cards[b].HideAfterMatch(delaySec);
        }
    }
}
