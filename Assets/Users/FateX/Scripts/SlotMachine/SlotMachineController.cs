using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Data;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.SlotMachine
{
    public class SlotMachineController : IInitializable, IDisposable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private RoundCurrency _roundCurrency;
        [Inject] private GameContext _gameContext;
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private CollectableHandler _collectableHandler;
        [Inject] private SlotMachineView _slotMachineView;
        [Inject] private GameStateManager _gameStateManager;
        
        private SlotMachinePrizeData _prizeData;
        private float _totalWeight;

        public void Initialize()
        {
            _slotMachineView.OnAnimationCompleted += HandleCompleted;
            CalculateTotalWeight();
        }

        public void Dispose()
        {
            _slotMachineView.OnAnimationCompleted -= HandleCompleted;
        }

        /// <summary>
        /// Вычисляет общий вес всех призов для взвешенной случайности
        /// </summary>
        private void CalculateTotalWeight()
        {
            _totalWeight = 0f;
            
            foreach (var prizeData in _gameConfig.GameConfigData.SlotMachinePrizeDatas)
            {
                _totalWeight += prizeData.Weight;
            }
        }

        /// <summary>
        /// Запускает розыгрыш с учетом весов призов
        /// </summary>
        public void Gambling()
        {
            if (_gameConfig.GameConfigData.SlotMachinePrizeDatas == null || 
                _gameConfig.GameConfigData.SlotMachinePrizeDatas.Length == 0)
            {
                UnityEngine.Debug.LogWarning("SlotMachinePrizeDatas пуст!");
                return;
            }

            SlotMachinePrizeData selectedPrize = GetWeightedRandomPrize();
            
            if (selectedPrize != null)
            {
                GetReward(selectedPrize);
            }
        }

        /// <summary>
        /// Выбирает приз на основе взвешенной случайности
        /// </summary>
        private SlotMachinePrizeData GetWeightedRandomPrize()
        {
            float randomValue = Random.Range(0f, _totalWeight);
            float cumulativeWeight = 0f;

            foreach (var prizeData in _gameConfig.GameConfigData.SlotMachinePrizeDatas)
            {
                cumulativeWeight += prizeData.Weight;
                
                if (randomValue <= cumulativeWeight)
                {
                    return prizeData;
                }
            }

            // Запасной вариант (не должен срабатывать при корректных данных)
            return _gameConfig.GameConfigData.SlotMachinePrizeDatas[0];
        }

        private void GetReward(SlotMachinePrizeData prizeData)
        {
            _slotMachineView.StartGambling(prizeData);
            _gameStateManager.ChangeState(GameStates.Gambling);
            _prizeData = prizeData;
        }

        private void HandleCompleted()
        {
            _gameStateManager.ChangeState(GameStates.Play);
            
            _gameContext.SnakeHealth.SetInvincible(1);

            
            switch (_prizeData.SlotMachinePrizeType)
            {
                case SlotMachinePrizeType.Coin:
                    _roundCurrency.AddCoin((int)_prizeData.Amount);
                    break;
                    
                case SlotMachinePrizeType.Heal:
                    _collectableHandler.UseHeal((int)_prizeData.Amount);
                    break;
                    
                case SlotMachinePrizeType.Xp:
                    _experienceSystem.AddExperiencePoints(_prizeData.Amount);
                    break;
                    
                case SlotMachinePrizeType.Magnet:
                    _collectableHandler.UseMagnet(null);
                    break;
                    
                case SlotMachinePrizeType.Segment:
                    if (_prizeData.SnakeSegmentBase != null && 
                        _prizeData.SnakeSegmentBase.Length > 0)
                    {
                        var prize = _prizeData.SnakeSegmentBase[
                            Random.Range(0, _prizeData.SnakeSegmentBase.Length)
                        ];
                        _gameContext.SnakeController.Grow(prize, false);
                    }
                    break;
            }
        }
    }
}