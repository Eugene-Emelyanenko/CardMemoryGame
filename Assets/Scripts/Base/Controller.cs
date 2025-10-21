using System;
using UnityEngine;
using Zenject;

namespace Base
{
    public abstract class Controller<TModel, TView>
        where TModel : Model
        where TView  : View
    {
        protected readonly TModel Model;
        protected readonly TView View;
        protected readonly SignalBus SignalBus;

        protected Controller(TModel model, TView view, SignalBus signalBus)
        {
            Model = model;
            View = view;
            SignalBus = signalBus;
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