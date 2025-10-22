using System;
using Zenject;

namespace Base
{
    public abstract class Controller<TModel, TView> : IInitializable, IDisposable
        where TModel : Model
        where TView  : View
    {
        protected readonly TModel Model;
        protected readonly TView View;

        protected SignalBus Bus { get; private set; }
        [Inject] private void InjectBus(SignalBus bus) => Bus = bus;

        protected Controller(TModel model, TView view)
        {
            Model = model;
            View  = view;
        }
        
        public virtual void Initialize()
        {
            Model.Initialize();
            View.Bind();
            OnAfterBind();
        }

        protected virtual void OnAfterBind() { }
        
        public virtual void Dispose()
        {
            View.Unbind();
            Model.Dispose();
        }
    }
}