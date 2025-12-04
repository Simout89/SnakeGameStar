using UnityEngine;
using Users.FateX.Scripts.CollectableItem;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/EnemyData")]
    public class EnemyData: ScriptableObject
    {
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public int Health { get; private set; } = 10;
        [field: SerializeField] public int MoveSpeed { get; private set; } = 10;
        [field: SerializeField] public int Damage { get; private set; } = 10;
        [field: SerializeField] public int AttackSpeed { get; private set; } = 10;
        [field: SerializeField] public Material OverrideMaterial { get; private set; }
        [field: SerializeField] public XpItem[] OverrideLootXP { get; private set; }
        [field: SerializeField] public GameObject[] ItemLoot { get; private set; }
    }
}