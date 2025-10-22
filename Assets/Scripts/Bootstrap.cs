using Controllers;
using Global;
using Models;
using Signals;
using Zenject;

public sealed class Bootstrap : IInitializable
{
    private readonly LoadingScreenController _loading;
    private readonly SpriteStorage _storage;
    private readonly SignalBus _bus;
    
    private readonly string _targetScene;

    public Bootstrap(
        LoadingScreenController loading,
        SpriteStorage storage,
        SignalBus bus,
        string sceneName = "MainMenu")
    {
        _loading = loading;
        _storage = storage;
        _bus = bus;
        _targetScene = sceneName;
    }

    public void Initialize()
    {
        _bus.Fire(new AudioSignals.PlayMusic("MainMenu", fade: 0.6f));

        _loading.LoadScene(
            sceneName: _targetScene,
            extraTask: _storage.PreloadAll()
        );
    }
}