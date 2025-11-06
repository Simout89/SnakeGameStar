using UnityEngine;
using Users.FateX.Scripts;
using Zenject;

namespace Скриптерсы.Zenject
{
    public class GamePlayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ActiveEntities>().AsSingle();
        }
    }
}