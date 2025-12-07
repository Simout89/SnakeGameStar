using System;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardSelectionHandler: IInitializable, IDisposable
    {
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameContext _gameContext;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        
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
            // _gameStateManager.ChangeState(GameStates.Play);
        }
    }
}