using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data.WaveData
{
    [CreateAssetMenu(menuName = "Data/WaveData")]
    public class WaveData: ScriptableObject
    {
        [field: SerializeField] public float TotalTime { get; private set; } = 600;
        [field: SerializeField] public WaveChangeSpawn[] WaveChangeSpawns;
    }
    
    
    [Serializable]
    public class WaveChangeSpawn
    {
        [field: SerializeField] public float TimeMarker { get; private set; } = 0; // от 0 до 1
        [field: SerializeField] public EnemyBase Enemy { get; private set; }
    }
}