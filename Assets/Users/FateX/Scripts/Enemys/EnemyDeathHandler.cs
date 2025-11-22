using System;
using Zenject;

namespace Users.FateX.Scripts.Enemys
{
    public class EnemyDeathHandler: IInitializable, IDisposable
    {
        [Inject] private EnemyManager _enemyManager;
        [Inject] private ExperienceFactory _experienceFactory;

        public void Initialize()
        {
            _enemyManager.OnEnemyDie += HandleEnemyDie;
        }

        public void Dispose()
        {
            _enemyManager.OnEnemyDie -= HandleEnemyDie;
        }

        private void HandleEnemyDie(EnemyBase obj)
        {
            _experienceFactory.SpawnXp(obj.transform.position);   
        }
    }
}