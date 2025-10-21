using UnityEngine;
using Zenject;

namespace Base
{
    public abstract class View : MonoBehaviour
    {
        protected bool _isBound;

        public virtual void Bind()
        {
            if (_isBound) return;
            _isBound = true;
        }

        public virtual void Unbind()
        {
            if (!_isBound) return;
            _isBound = false;
        }

        public virtual void Show()  => gameObject.SetActive(true);
        public virtual void Hide()  => gameObject.SetActive(false);

        protected virtual void OnDestroy()
        {
            if (_isBound) Unbind();
        }
    }
}