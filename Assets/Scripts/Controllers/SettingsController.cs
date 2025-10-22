using Base;
using Models;
using Views;
using Signals;

namespace Controllers
{
    public class SettingsController : Controller<SettingsModel, SettingsView>
    {
        public SettingsController(SettingsModel model, SettingsView view)
            : base(model, view) { }

        public override void Initialize()
        {
            base.Initialize();

            View.SetMusicValue(Model.MusicVolume);
            View.SetSfxValue(Model.SfxVolume);
            View.Hide(0f);

            Bus.Subscribe<SettingsSignals.MusicChanged>(OnMusic);
            Bus.Subscribe<SettingsSignals.SfxChanged>(OnSfx);
            Bus.Subscribe<SettingsSignals.BackClicked>(OnBack);
            Bus.Subscribe<MainMenuSignals.SettingsClicked>(OnSettings);
        }

        public override void Dispose()
        {
            Bus.TryUnsubscribe<SettingsSignals.MusicChanged>(OnMusic);
            Bus.TryUnsubscribe<SettingsSignals.SfxChanged>(OnSfx);
            Bus.TryUnsubscribe<SettingsSignals.BackClicked>(OnBack);
            Bus.TryUnsubscribe<MainMenuSignals.SettingsClicked>(OnSettings);
            base.Dispose();
        }
        
        private void OnSettings(MainMenuSignals.SettingsClicked _) => View.Show();
        private void OnMusic(SettingsSignals.MusicChanged s) => Model.SetMusic(s.Value);
        private void OnSfx(SettingsSignals.SfxChanged s) => Model.SetSfx(s.Value);

        private void OnBack(SettingsSignals.BackClicked _)
        {
            Model.Save();
            View.Hide(0f);
        }
    }
}