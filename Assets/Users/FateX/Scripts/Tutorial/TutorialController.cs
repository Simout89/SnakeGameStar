using System;
using Cysharp.Threading.Tasks;
using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts.Tutorial
{
    public class TutorialController: IInitializable, IDisposable
    {
        [Inject] private GamePlaySceneEntryPoint _gamePlaySceneEntryPoint;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private TutorialView _tutorialView;
        [Inject] private SettingsController _settingsController;

        public void ShowTutorial(TutorialWindowType tutorialWindowType)
        {
            _gameStateManager.PushState(GameStates.Tutorial);

            _tutorialView.ShowWindow(tutorialWindowType);
            
            if(tutorialWindowType == TutorialWindowType.Move)
                ShowXpItem().Forget();
        }

        public void Initialize()
        {
            GameEvents.OnEnemyDie += HandleEnemyDie;
        }

        public void Dispose()
        {
            GameEvents.OnEnemyDie -= HandleEnemyDie;
        }

        private void HandleEnemyDie(EnemyData arg1, DamageInfo arg2)
        {
            if (!_settingsController.SettingsSaveData.KillTutorial)
            {
                ShowTutorial(TutorialWindowType.KillEnemy);
                _settingsController.SettingsSaveData.KillTutorial = true;
                _settingsController.SaveSettings();
            }
        }

        private async UniTaskVoid ShowXpItem()
        {
            await UniTask.WaitForSeconds(20);
            ShowTutorial(TutorialWindowType.XpHealth);
        }
    }
}