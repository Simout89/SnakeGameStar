using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
    public class PoolTrailCleaner: MonoBehaviour, IPoolable
    {
        [SerializeField] private TrailRenderer[] _trailRenderers;
        
        public void OnSpawn()
        {
            foreach (var trail in _trailRenderers)
            {
                trail.Clear();
            }  
        }

        public void OnDespawn()
        {
            foreach (var trail in _trailRenderers)
            {
                trail.Clear();
            }
        }
    }
}