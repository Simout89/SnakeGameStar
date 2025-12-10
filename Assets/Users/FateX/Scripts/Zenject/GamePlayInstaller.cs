using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Enemy;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.Entity;
using Users.FateX.Scripts.Shop;
using Users.FateX.Scripts.SlotMachine;
using Users.FateX.Scripts.Trial;
using Users.FateX.Scripts.Tutorial;
using Users.FateX.Scripts.View;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы.Zenject
{
    public class GamePlayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSpawners();
            BindViews();
            BindCoreSystems();
            BindFactories();
            BindGameplayLogic();
        }

        private void BindSpawners()
        {
            Container.BindInterfacesAndSelfTo<EnemySpawnArea>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SnakeSpawner>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().FromComponentsInHierarchy().AsSingle();
        }

        private void BindViews()
        {
            Container.BindInterfacesAndSelfTo<CardMenuView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HealthView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<DeathView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SlotMachineView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<MessageDisplayView>().FromComponentsInHierarchy().AsSingle();
        }

        private void BindCoreSystems()
        {
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActiveEntities>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlaySceneEntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnDirector>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameContext>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonoHelper>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalSoundPlayer>().AsSingle();
            Container.BindInterfacesAndSelfTo<AchievementController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<TutorialController>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrialTowerFactory>().AsSingle();
        }

        private void BindGameplayLogic()
        {
            Container.BindInterfacesAndSelfTo<CollectableHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExperienceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyDeathHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelUpHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoundCurrency>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatisticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlotMachineController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrialWaveDirector>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrialSpawnDirector>().AsSingle();

            Container.BindInterfacesAndSelfTo<CardMenuController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CardSelectionHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AchievementManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DeathHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<DamageShow>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopController>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyManager>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}