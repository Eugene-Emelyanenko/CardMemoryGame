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
            View.SetMasterValue(Model.MasterVolume);
            View.Hide(0f);

            Bus.Subscribe<SettingsSignals.MusicChanged>(OnMusic);
            Bus.Subscribe<SettingsSignals.SfxChanged>(OnSfx);
            Bus.Subscribe<SettingsSignals.MasterChanged>(OnMaster);
            
            Bus.Subscribe<SettingsSignals.BackClicked>(OnBack);
            Bus.Subscribe<MainMenuSignals.SettingsClicked>(OnSettings);
        }

        public override void Dispose()
        {
            Bus.TryUnsubscribe<SettingsSignals.MusicChanged>(OnMusic);
            Bus.TryUnsubscribe<SettingsSignals.SfxChanged>(OnSfx);
            Bus.TryUnsubscribe<SettingsSignals.MasterChanged>(OnMaster);
            
            Bus.TryUnsubscribe<SettingsSignals.BackClicked>(OnBack);
            Bus.TryUnsubscribe<MainMenuSignals.SettingsClicked>(OnSettings);
            
            base.Dispose();
        }
        
        private void OnSettings(MainMenuSignals.SettingsClicked _) => View.Show();

        private void OnMusic(SettingsSignals.MusicChanged s)
        {
            Model.SetMusic(s.Value);
            Bus.Fire(new AudioSignals.SetMusicVolume(s.Value));
        }

        private void OnSfx(SettingsSignals.SfxChanged s)
        {
            Model.SetSfx(s.Value);
            Bus.Fire(new AudioSignals.SetSfxVolume(s.Value));
        }
        
        private void OnMaster(SettingsSignals.MasterChanged s)
        {
            Model.SetMaster(s.Value);
            Bus.Fire(new AudioSignals.SetMasterVolume(s.Value));
        }

        private void OnBack(SettingsSignals.BackClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            Model.Save();
            View.Hide(0f);
        }
    }
}