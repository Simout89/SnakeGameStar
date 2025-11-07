using System.Collections.Generic;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeHealth: MonoBehaviour
    {
        [SerializeField] private List<SnakeBodyPartHealth> snakeBodyPartHealths = new List<SnakeBodyPartHealth>();

        public void Add(SnakeBodyPartHealth snakeBodyPartHealth)
        {
            snakeBodyPartHealths.Add(snakeBodyPartHealth);

            snakeBodyPartHealth.OnHealthChanged += HandleHealthChanged;
        }


        public void Remove(SnakeBodyPartHealth snakeBodyPartHealth)
        {
            snakeBodyPartHealth.OnHealthChanged -= HandleHealthChanged;
            
            snakeBodyPartHealths.Remove(snakeBodyPartHealth);
        }
        private void HandleHealthChanged(float obj)
        {
            Debug.Log("Змея получила урон");
        }
    }
}