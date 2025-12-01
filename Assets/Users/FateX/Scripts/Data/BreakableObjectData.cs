using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/Entity/BreakableObjectData")]
    public class BreakableObjectData: ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 10f;
        [field: SerializeField] public GameObject[] loot;
    }
}