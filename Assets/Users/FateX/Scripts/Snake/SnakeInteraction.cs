using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeInteraction: MonoBehaviour
    {
        [SerializeField] private Snake snake;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                snake.Grow();
            }
        }
    }
}