using Users.FateX.Scripts;
using Users.FateX.Scripts.Services;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы.Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStats>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameConfig>().AsSingle();
            Container.BindInterfacesAndSelfTo<SnakeSegmentsRepository>().AsSingle();

        }

    }

}