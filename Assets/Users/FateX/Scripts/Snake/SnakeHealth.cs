using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts
{
    public class SnakeHealth: MonoBehaviour
    {
        private List<SnakeSegmentBase> snakeBodyParts = new List<SnakeSegmentBase>();

        private float timeToNextHit = 0;

        public void Add(SnakeSegmentBase snakeSegmentBase)
        {
            snakeBodyParts.Add(snakeSegmentBase);

            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage += HandleTakeDamage;
        }
        
        public void Remove(SnakeSegmentBase snakeSegmentBase)
        {
            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            
            snakeBodyParts.Remove(snakeSegmentBase);
        }

        private void OnDisable()
        {
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            }
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            if(Time.time < timeToNextHit)
                return;

            timeToNextHit = Time.time + 0.16f;
            
            Debug.Log("УРон");
            
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.DamageEffect();
            }
        }
    }
}