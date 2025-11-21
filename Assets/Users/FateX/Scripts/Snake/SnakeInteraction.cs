using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Users.FateX.Scripts
{
    public class SnakeInteraction: MonoBehaviour
    {
        [FormerlySerializedAs("snake")] [SerializeField] private SnakeController snakeController;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                // snakeController.Grow();
            }
        }
    }
}