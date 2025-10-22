using Base;
using Models;
using Views;
using Signals;

namespace Controllers
{
    public class PauseController : Controller<PauseModel, PauseView>
    {
        private readonly LoadingScreenController _loading;

        public PauseController(
            PauseModel model,
            PauseView view,
            LoadingScreenController loading)
            : base(model, view)
        {
            _loading = loading;
        }
        
        public override void Initialize()
        {
            base.Initialize();

            View.Hide(0f);

            Bus.Subscribe<PauseSignals.PauseClicked>(OnPause);
            Bus.Subscribe<PauseSignals.ContinueClicked>(OnContinue);
            Bus.Subscribe<PauseSignals.MenuClicked>(OnMenu);
        }

        public override void Dispose()
        {
            Bus.TryUnsubscribe<PauseSignals.PauseClicked>(OnPause);
            Bus.TryUnsubscribe<PauseSignals.ContinueClicked>(OnContinue);
            Bus.TryUnsubscribe<PauseSignals.MenuClicked>(OnMenu);
            
            base.Dispose();
        }
        
        private void OnPause(PauseSignals.PauseClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            View.Show();
        }
        
        private void OnContinue(PauseSignals.ContinueClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            View.Hide();
        }

        private void OnMenu(PauseSignals.MenuClicked _)
        {
            Bus.Fire(new AudioSignals.PlaySfx("Click"));
            _loading.LoadScene(Model.TargetGameScene);
        }
    }
}