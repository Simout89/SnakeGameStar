using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts
{
    public class SnakeHealth: MonoBehaviour
    {
        private List<SnakeSegmentBase> snakeBodyParts = new List<SnakeSegmentBase>();

        public void Add(SnakeSegmentBase snakeSegmentBase)
        {
            snakeBodyParts.Add(snakeSegmentBase);

            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage += HandleTakeDamage;
            
            Debug.Log("Подписка");
        }
        
        public void Remove(SnakeSegmentBase snakeSegmentBase)
        {
            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            
            snakeBodyParts.Remove(snakeSegmentBase);
            
            Debug.Log("Отписка");
        }

        private void OnDisable()
        {
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            }
            
            Debug.Log("Отписка");
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            Debug.Log("Змея получила урон");

            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.DamageEffect();
            }
        }
    }
}