using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SnakeData")]
    public class SnakeData: ScriptableObject
    {
        [field: SerializeField] public float BaseMoveSpeed { get; private set; } = 5;
        [field: SerializeField] public float BaseRotateSpeed { get; private set; } = 5;
        [field: SerializeField] public float BaseHealth { get; private set; } = 10;
        [field: SerializeField] public float SegmentDistance { get; private set; } = 3;
    }
}