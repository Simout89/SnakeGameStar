using TMPro;
using UnityEngine;

namespace Users.FateX.Scripts.View
{
    public class DamageView: MonoBehaviour
    {
        [field: SerializeField] public TMP_Text TMPText { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    }
}