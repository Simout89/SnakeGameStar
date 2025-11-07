using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SnakeData")]
    public class SnakeData: ScriptableObject
    {
        [field: SerializeField] public int BaseMoveSpeed { get; private set; } = 5;
        [field: SerializeField] public int BaseRotateSpeed { get; private set; } = 5;
        [field: SerializeField] public int BaseHealth { get; private set; } = 10;
        [field: SerializeField] public int BaseSegmentCount { get; private set; } = 3;
    }
}