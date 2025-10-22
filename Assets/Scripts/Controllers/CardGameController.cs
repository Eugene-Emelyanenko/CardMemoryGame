using System.Collections;
using System.Collections.Generic;
using Base;
using Card;
using DG.Tweening;
using Global;
using Models;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Controllers
{
    public class CardGameController : Controller<CardGameModel, CardGameView>
    {
        private readonly GlobalCoroutineRunner _runner;

        private int _first   = -1;
        private int _matches = 0;
        private bool _busy   = false;

        public CardGameController(CardGameModel model, CardGameView view, GlobalCoroutineRunner runner)
            : base(model, view)
        {
            _runner = runner;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.Show(0f);

            _runner.Run(WaitAndBuild());
            Bus.Subscribe<CardGameSignals.CardClicked>(OnCardClicked);
        }

        public override void Dispose()
        {
            Bus.TryUnsubscribe<CardGameSignals.CardClicked>(OnCardClicked);
            base.Dispose();
        }

        private IEnumerator WaitAndBuild()
        {
            BuildRound();
            
            View.RevealAllImmediate();
            yield return new WaitForSeconds(Model.showTime);
            View.ConcealAllImmediate();
        }

        private void BuildRound()
        {
            _busy = true;
            _first = -1;
            _matches = 0;
            
            var faces = new List<Sprite>();
            var ids   = new List<string>();

            var pool = new List<CardData>(Model.GameCards);
            Shuffle(pool);

            for (int i = 0; i < 3; i++)
            {
                var cd = pool[i];
                faces.Add(cd.Sprite);
                ids.Add(cd.Id);

                faces.Add(cd.Sprite);
                ids.Add(cd.Id);
            }

            ShuffleParallel(ids, faces);

            View.BuildSlots(6, ids, faces, Model.CardBack.Sprite);
            View.SetCounter(0, 3);

            _busy = false;
        }

        private void OnCardClicked(CardGameSignals.CardClicked s)
        {
            if (_busy) return;
            if (s.Index < 0 || s.Index >= View.Count) return;
            if (!View.IsActive(s.Index)) return;
            if (View.IsFront(s.Index))  return;

            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            
            if (_first == -1)
            {
                View.FlipToFront(s.Index);
                _first = s.Index;
                return;
            }
            
            var a = _first;
            var b = s.Index;
            _first = -1;
            _busy = true;

            Tween flipTween = View.FlipToFrontTween(b);
            _runner.Run(ResolveAfterSecondFlip(a, b, flipTween));
        }

        private IEnumerator ResolveAfterSecondFlip(int a, int b, Tween secondFlipTween)
        {
            if (secondFlipTween != null)
                yield return secondFlipTween.WaitForCompletion();

            if (View.GetId(a) == View.GetId(b))
            {
                _matches++;
                View.SetCounter(_matches, Model.totalPairs);
                
                View.HideMatched(a, b, Model.waitTime);
                yield return new WaitForSeconds(Model.waitTime);

                _busy = false;
                if (_matches >= Model.totalPairs)
                {
                    Bus.Fire(new AudioSignals.PlaySfx("Win"));
                    _runner.Run(RestartRound());
                }
                else
                {
                    Bus.Fire(new AudioSignals.PlaySfx("Correct"));
                }
            }
            else
            {
                yield return new WaitForSeconds(Model.waitTime);
                Bus.Fire(new AudioSignals.PlaySfx("Wrong"));
                View.FlipToBack(a);
                View.FlipToBack(b);
                _busy = false;
            }
        }

        private IEnumerator RestartRound()
        {
            _busy = true;
            yield return new WaitForSeconds(Model.waitTime);
            BuildRound();

            View.RevealAllImmediate();
            yield return new WaitForSeconds(Model.showTime);
            View.ConcealAllImmediate();

            _busy = false;
        }
        
        private static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private static void ShuffleParallel<T1, T2>(IList<T1> a, IList<T2> b)
        {
            for (int i = a.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (a[i], a[j]) = (a[j], a[i]);
                (b[i], b[j]) = (b[j], b[i]);
            }
        }
    }
}
