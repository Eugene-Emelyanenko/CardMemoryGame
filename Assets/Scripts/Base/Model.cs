using UnityEngine;
using Zenject;

namespace Base
{
    public abstract class Model
    {
        protected readonly SignalBus SignalBus;

        protected Model(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }

        public virtual void Initialize() { }

        public virtual void Dispose() { }
    }
}