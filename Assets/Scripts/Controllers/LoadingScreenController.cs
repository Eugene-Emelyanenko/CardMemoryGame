using System;
using System.Collections;
using Base;
using DontDestroyOnLoad;
using Models;
using UnityEngine;
using Views;
using Zenject;

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

        public void LoadScene(string sceneName, Action onCompleted = null)
        {
            _runner.Run(LoadFlow(sceneName, onCompleted));
        }

        private IEnumerator LoadFlow(string sceneName, Action onCompleted)
        {
            View.Show();
            View.SetProgress(0f);

            yield return Model.LoadSceneRoutine(sceneName, p => View.SetProgress(p));

            View.Hide();
            onCompleted?.Invoke();
        }
    }
}