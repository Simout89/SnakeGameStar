using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Data;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.SlotMachine
{
    public class SlotMachineController
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private RoundCurrency _roundCurrency;
        [Inject] private GameContext _gameContext;
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private CollectableHandler _collectableHandler;
        [Inject] private SlotMachineView _slotMachineView;

        public void Gambling()
        {
            
            int prize = Random.Range(0, Enum.GetValues(typeof(SlotMachinePrizeType)).Length + 1);
            
            SlotMachinePrizeType prizeType = (SlotMachinePrizeType)prize;

            foreach (var prizeData in _gameConfig.GameConfigData.SlotMachinePrizeDatas)
            {
                if (prizeData.SlotMachinePrizeType == (SlotMachinePrizeType)prize)
                {
                    GetReward(prizeData);
                }
            }
        }

        private void GetReward(SlotMachinePrizeData prizeData)
        {
            switch (prizeData.SlotMachinePrizeType)
            {
                case SlotMachinePrizeType.Coin:
                {
                    _roundCurrency.AddCoin((int)prizeData.Amount);
                }
                    break;
                case SlotMachinePrizeType.Heal:
                {
                    _collectableHandler.UseHeal((int)prizeData.Amount);
                }
                    break;
                case SlotMachinePrizeType.Xp:
                {
                    _experienceSystem.AddExperiencePoints(prizeData.Amount);
                }
                    break;
                case SlotMachinePrizeType.Magnet:
                {
                    _collectableHandler.UseMagnet(null);
                }
                    break;
                case SlotMachinePrizeType.Segment:
                {
                    var prize = prizeData.SnakeSegmentBase[Random.Range(0, prizeData.SnakeSegmentBase.Length)];
                    _gameContext.SnakeController.Grow(prize);
                }
                    break;
            }
        }
    }
}