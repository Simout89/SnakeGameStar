using UnityEngine;

namespace Users.FateX.Scripts.Data.Enity
{
    [CreateAssetMenu(menuName = "Data/Entity/XpEntityData")]
    public class XpEntityData: ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int XpAmount { get; private set; } = 10;
    }
}