using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/EnemyData")]
    public class EnemyData: ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; } = 10;
        [field: SerializeField] public int MoveSpeed { get; private set; } = 10;
        [field: SerializeField] public int Damage { get; private set; } = 10;
        [field: SerializeField] public int AttackSpeed { get; private set; } = 10;
    }
}