using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Enemy;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.Entity;
using Users.FateX.Scripts.View;
using Zenject;

namespace Скриптерсы.Zenject
{
    public class GamePlayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemySpawnArea>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SnakeSpawner>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CardMenuView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HealthView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<DeathView>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemyManager>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ActiveEntities>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlaySceneEntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnDirector>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameContext>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollectableHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExperienceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyDeathHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelUpHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CardMenuController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CardSelectionHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AchievementManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DeathHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<DamageShow>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameConfig>().AsSingle();
        }
    }
}