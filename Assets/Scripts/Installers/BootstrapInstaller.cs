using Zenject;
using UnityEngine;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrap>()
                .AsSingle()
                .NonLazy();
        }
    }
}