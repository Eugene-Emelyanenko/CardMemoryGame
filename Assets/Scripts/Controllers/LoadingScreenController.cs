using System;
using System.Collections;
using Base;
using DG.Tweening;
using Global;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;

namespace Controllers
{
    public class LoadingScreenController : Controller<LoadingScreenModel, LoadingScreenView>
    {
        private readonly GlobalCoroutineRunner _runner;

        public LoadingScreenController(
            LoadingScreenModel model,
            LoadingScreenView view,
            GlobalCoroutineRunner runner)
            : base(model, view)
        {
            _runner = runner;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.Hide(0f);
        }

        public void LoadScene(
            string sceneName,
            Action onCompleted = null,
            IEnumerator extraTask = null,
            float taskWeight = 0.6f)
        {
            _runner.Run(LoadFlow(sceneName, onCompleted, extraTask, taskWeight));
        }
        
        private IEnumerator LoadFlow(
            string sceneName,
            Action onCompleted,
            IEnumerator extraTask,
            float taskWeight)
        {
            taskWeight = Mathf.Clamp01(taskWeight);

            View.Show();
            View.SetProgress(0f);
            
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            
            bool taskDone = true;
            if (extraTask != null)
            {
                taskDone = false;
                _runner.Run(TaskWrapper(extraTask, () => taskDone = true));
            }

            float elapsed = 0f;
            float visual  = 0f;

            while (!(elapsed >= Model.minDuration && op.progress >= 0.9f && taskDone))
            {
                elapsed += Time.unscaledDeltaTime;
                
                float timeFloor = Mathf.Clamp01(elapsed / Model.minDuration) * 0.99f;
                
                if (timeFloor > visual)
                {
                    visual = timeFloor;
                    View.SetProgress(visual);
                }

                yield return null;
            }
            
            yield return DOTween
                .To(() => visual, x => { visual = x; View.SetProgress(visual); }, 1f, 0.25f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .WaitForCompletion();
            
            op.allowSceneActivation = true;

            View.Hide();
            onCompleted?.Invoke();
        }
        
        private static IEnumerator TaskWrapper(IEnumerator routine, Action onDone)
        {
            while (true)
            {
                bool hasNext;
                try { hasNext = routine.MoveNext(); }
                catch (Exception e)
                {
                    Debug.LogError($"[LoadingScreen] Extra task threw: {e}");
                    break;
                }

                if (!hasNext) break;
                yield return routine.Current;
            }

            onDone?.Invoke();
        }
    }
}