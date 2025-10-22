using Controllers;
using Signals;
using UnityEngine;
using Zenject;

public sealed class Bootstrap : IInitializable
{
    private readonly LoadingScreenController _loading;
    private readonly string _targetScene;
    private readonly SignalBus Bus;
    
    public Bootstrap(LoadingScreenController loading, SignalBus signalBus, string sceneName = "MainMenu")
    {
        _loading = loading;
        Bus = signalBus;
        _targetScene = sceneName;
    }

    public void Initialize()
    {
        Bus.Fire(new AudioSignals.PlayMusic("MainMenu", fade: 0.6f));
        _loading.LoadScene(_targetScene);
    }
}
