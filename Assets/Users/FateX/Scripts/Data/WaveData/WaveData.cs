using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data.WaveData
{
    [CreateAssetMenu(menuName = "Data/WaveData")]
    public class WaveData: ScriptableObject
    {
        [field: SerializeField] public float TotalTime { get; private set; } = 600;
        [field: SerializeField] public WaveChangeSpawn[] WaveChangeSpawns;
        [field: SerializeField] public WaveEventSpawn[] WaveEventSpawns;
    }
    
    
    [Serializable]
    public class WaveChangeSpawn
    {
        [field: SerializeField] public float TimeMarker { get; private set; } = 0; // от 0 до 1
        [field: SerializeField] public EnemyBase[] Enemy { get; private set; }
    }

    [Serializable]
    public class WaveEventSpawn
    {
        [field: SerializeField] public float TimeMarker { get; private set; } = 0;
        [field: SerializeField] public EnemyBase[] Enemys { get; private set; }
        [field: SerializeField] public float OverrideSpawnRate { get; private set; } = -1;
        [field: SerializeField] public float MultiplaySpawnRate { get; private set; } = -1;
    }
}