using System;
using System.Linq;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.SlotMachine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.CollectableItem
{
    public class CollectableHandler: IDisposable
    {
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private ItemManager _itemManager;
        [Inject] private GameContext _gameContext;
        [Inject] private RoundCurrency _roundCurrency;
        [Inject] private SlotMachineController _slotMachineController;
        
        private SnakeInteraction _snakeInteraction;
        
        public int HealItemUsed { get; private set; }
        public int MagnetItemUsed { get; private set; }
        
        public void SetSnakeInteraction(SnakeInteraction snakeInteraction)
        {
            _snakeInteraction = snakeInteraction;
            
            snakeInteraction.OnCollect += HandleCollect;
        }

        public void Dispose()
        {
            _snakeInteraction.OnCollect -= HandleCollect;
        }

        private void HandleCollect(GameObject obj)
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.PickUp);
            
            HomingMover.StartMove(obj.transform, _snakeInteraction.transform, () =>
            {
                if (obj.TryGetComponent(out IExperiencePoints experiencePoints))
                {
                    _experienceSystem.AddExperiencePoints(experiencePoints);
                
                    _itemManager.RemoveXpItem(obj.GetComponent<XpItem>());
                }
            
                if (obj.TryGetComponent(out IMagnet magnet ))
                {
                    UseMagnet(obj);
                    
                }

                if (obj.TryGetComponent(out ICoin coin))
                {
                    _roundCurrency.AddCoin(coin.CoinAmount);
                    _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.CollectCoin);

                }
            
                if (obj.TryGetComponent(out IHealableItem healable))
                {
                    UseHeal(healable);
                    _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.EatApple);

                }
            
                if (obj.TryGetComponent(out IGamblingItem gamblingItem))
                {
                    _slotMachineController.Gambling();
                }
                
                LeanPool.Despawn(obj);
            });
            
            // LeanPool.Despawn(obj);
        }

        public void UseHeal(IHealableItem healable)
        {
            _gameContext.SnakeHealth.Heal(healable.Value);
            HealItemUsed++;
        }
        
        public void UseHeal(float value)
        {
            _gameContext.SnakeHealth.Heal(value);
            HealItemUsed++;
        }

        public void UseMagnet(GameObject obj)
        {
            var xpItems = _itemManager.GetXpItemsArray().ToArray();
    
            foreach (var xpItem in xpItems)
            {
                if (xpItem == null) continue;
        
                HomingMover.StartMove(xpItem.transform, _snakeInteraction.transform, () =>
                {
                    if (xpItem != null) 
                    {
                        _experienceSystem.AddExperiencePoints(xpItem);
                        LeanPool.Despawn(xpItem.gameObject);
                    }
                });
        
                _itemManager.RemoveXpItem(xpItem.GetComponent<XpItem>());
            }

            MagnetItemUsed++;
        }
    }
}