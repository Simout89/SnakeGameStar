using System;
using Users.FateX.Scripts.Tutorial;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardSelectionHandler: IInitializable, IDisposable
    {
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameContext _gameContext;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private TutorialController _tutorialController;
        [Inject] private SettingsController _settingsController;
        
        public void Initialize()
        {
            _cardMenuController.OnCardSelected += HandleSelected;
        }

        public void Dispose()
        {
            _cardMenuController.OnCardSelected -= HandleSelected;
        }

        private void HandleSelected()
        {
            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.CardSelected);

            _gameContext.SnakeHealth.SetInvincible(1);
            
            
            if(!_settingsController.SettingsSaveData.MoveTutorial)
            {
                _tutorialController.ShowTutorial(TutorialWindowType.Move);
                _settingsController.SettingsSaveData.MoveTutorial = true;
            }
            // _gameStateManager.ChangeState(GameStates.Play);
        }
    }
}