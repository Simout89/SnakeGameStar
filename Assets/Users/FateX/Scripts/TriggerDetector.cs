using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class TriggerDetector : MonoBehaviour
    {
        public event Action<Collider> onTriggerEntered;
        public event Action<Collider> onTriggerStayed;
        public event Action<Collider> onTriggerExited;
    
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEntered?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStayed?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExited?.Invoke(other);
        }
    }
}