using Users.FateX.Scripts;
using Users.FateX.Scripts.Services;
using Users.FateX.Scripts.Tutorial;
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
            Container.BindInterfacesAndSelfTo<SettingsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SnakeSegmentsRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonoHelper>().FromNewComponentOnNewGameObject().AsSingle();
        }

    }

}