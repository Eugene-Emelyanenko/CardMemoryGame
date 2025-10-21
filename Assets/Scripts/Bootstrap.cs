using Controllers;
using UnityEngine;
using Zenject;

public sealed class Bootstrap : IInitializable
{
    private readonly LoadingScreenController _loading;
    private readonly string _targetScene;
    
    public Bootstrap(LoadingScreenController loading, string sceneName = "MainMenu")
    {
        _loading = loading;
        _targetScene = sceneName;
    }

    public void Initialize()
    {
        _loading.LoadScene(_targetScene);
    }
}
