using System;
using Users.FateX.Scripts.Cards;
using Zenject;

namespace Users.FateX.Scripts
{
    public class LevelUpHandler : IInitializable, IDisposable
    {
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;

        private int _pendingLevelUps = 0;

        public void Initialize()
        {
            _experienceSystem.OnGetLevel += HandleGetLvl;
            _cardMenuController.OnCardSelected += HandleCardSelected;
        }

        public void Dispose()
        {
            _experienceSystem.OnGetLevel -= HandleGetLvl;
            _cardMenuController.OnCardSelected -= HandleCardSelected;
        }

        private void HandleGetLvl()
        {
            _pendingLevelUps++;

            TryOpenMenu();
        }

        private void HandleCardSelected()
        {
            _gameStateManager.PopState();

            TryOpenMenu();
        }

        private void TryOpenMenu()
        {
            if (_pendingLevelUps <= 0)
                return;

            if (_gameStateManager.CurrentState == GameStates.CardMenu)
                return;

            _pendingLevelUps--;

            _cardMenuController.SpawnRandomCards();
            _gameStateManager.PushState(GameStates.CardMenu);
        }
    }

}