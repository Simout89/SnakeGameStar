using TMPro;
using UnityEngine;

namespace Users.FateX.Scripts.View
{
    public class LairView: MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionField;

        public void SetDescription(string text)
        {
            descriptionField.text = text;
        }
    }
}