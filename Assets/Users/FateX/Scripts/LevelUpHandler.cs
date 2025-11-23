using System;
using Users.FateX.Scripts.Cards;
using Zenject;

namespace Users.FateX.Scripts
{
    public class LevelUpHandler: IInitializable, IDisposable
    {
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        
        public void Initialize()
        {
            _experienceSystem.OnGetLevel += HandleGetLvl;
        }

        public void Dispose()
        {
            _experienceSystem.OnGetLevel -= HandleGetLvl;
        }

        private void HandleGetLvl()
        {
            _cardMenuController.SpawnRandomCards();
            
            _gameStateManager.ChangeState(GameStates.CardMenu);
        }
    }
}