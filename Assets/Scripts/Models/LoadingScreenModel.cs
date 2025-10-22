using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Zenject;
using Base;

namespace Models
{
    public class LoadingScreenModel : Model
    {
        public IEnumerator LoadSceneRoutine(
            string sceneName,
            Action<float> onProgress,
            float minDurationSeconds = 3f)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            float elapsed = 0f;
            float visualProgress = 0f;
            
            onProgress?.Invoke(0f);
            
            Tween tween = DOTween.To(
                () => visualProgress,
                x =>
                {
                    visualProgress = x;
                    onProgress?.Invoke(visualProgress);
                },
                0.99f,
                minDurationSeconds
            ).SetEase(Ease.Linear);
            
            while (elapsed < minDurationSeconds || op.progress < 0.9f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            if (tween.active && tween.IsPlaying())
                tween.Complete();
            
            yield return DOTween.To(
                () => visualProgress,
                x =>
                {
                    visualProgress = x;
                    onProgress?.Invoke(visualProgress);
                },
                1f,
                0.3f
            ).SetEase(Ease.Linear)
             .WaitForCompletion();

            op.allowSceneActivation = true;
        }
    }
}
