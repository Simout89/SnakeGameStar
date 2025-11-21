using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class TriggerDetector : MonoBehaviour
    {
        public event Action<Collider2D> onTriggerEntered;
        public event Action<Collider2D> onTriggerStayed;
        public event Action<Collider2D> onTriggerExited;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEntered?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            onTriggerStayed?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExited?.Invoke(other);
        }
    }
}