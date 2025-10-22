using Zenject;

namespace Base
{
    public abstract class Model
    {
        protected SignalBus Bus { get; private set; }

        [Inject]
        private void InjectBus(SignalBus bus) => Bus = bus;

        public virtual void Initialize() { }
        public virtual void Dispose() { }
    }
}