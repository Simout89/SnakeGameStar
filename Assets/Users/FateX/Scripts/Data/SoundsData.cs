using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        [field: SerializeField] public AK.Wwise.Event DamageSound { get; private set; }
    }
}