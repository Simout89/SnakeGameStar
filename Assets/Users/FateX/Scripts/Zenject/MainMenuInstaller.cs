using Users.FateX.Scripts;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Shop;
using Zenject;

namespace Скриптерсы.Zenject
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShopView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopController>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<MainMenuSceneEntryPoint>().AsSingle();
            
            
            
            Container.BindInterfacesAndSelfTo<AchievementController>().AsSingle().NonLazy();
            Container.Bind<IAchievementView>().To<AchievementView>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MonoHelper>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalSoundPlayer>().AsSingle();

        }
    }
}