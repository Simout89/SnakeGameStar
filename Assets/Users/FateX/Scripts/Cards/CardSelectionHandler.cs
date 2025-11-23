using System;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardSelectionHandler: IInitializable, IDisposable
    {
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        
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
            _gameStateManager.ChangeState(GameStates.Play);
        }
    }
}