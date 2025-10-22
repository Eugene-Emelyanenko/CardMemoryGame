using Base;
using Models;
using Views;
using Signals;

namespace Controllers
{
    public class MainMenuController : Controller<MainMenuModel, MainMenuView>
    {
        private readonly LoadingScreenController _loading;

        public MainMenuController(
            MainMenuModel model,
            MainMenuView view,
            LoadingScreenController loading)
            : base(model, view)
        {
            _loading  = loading;
        }

        public override void Initialize()
        {
            base.Initialize();

            View.Show(0f);

            Bus.Subscribe<MainMenuSignals.PlayClicked>(OnPlay);
            Bus.Subscribe<MainMenuSignals.SettingsClicked>(OnSettings);
            Bus.Subscribe<SettingsSignals.BackClicked>(OnBack);
        }

        public override void Dispose()
        {
            Bus.TryUnsubscribe<MainMenuSignals.PlayClicked>(OnPlay);
            Bus.TryUnsubscribe<MainMenuSignals.SettingsClicked>(OnSettings);
            Bus.TryUnsubscribe<SettingsSignals.BackClicked>(OnBack);
            base.Dispose();
        }

        private void OnBack(SettingsSignals.BackClicked _)
        {
            View.Show();
        }
        
        private void OnPlay(MainMenuSignals.PlayClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            Bus.Fire(new AudioSignals.PlayMusic("Game", fade: 0.6f));
            _loading.LoadScene(Model.TargetGameScene);
        }

        private void OnSettings(MainMenuSignals.SettingsClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            View.Hide(0f);
        }
    }
}