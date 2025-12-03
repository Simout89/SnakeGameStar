using System;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardSelectionHandler: IInitializable, IDisposable
    {
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameContext _gameContext;
        
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
            _gameContext.SnakeHealth.SetInvincible(1);
            _gameStateManager.ChangeState(GameStates.Play);
        }
    }
}