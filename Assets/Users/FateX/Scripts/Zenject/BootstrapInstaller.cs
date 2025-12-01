using Users.FateX.Scripts;
using Zenject;

namespace Скриптерсы.Zenject
{
    public class BootstrapInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrap>().AsSingle().NonLazy();
        }
    }
}