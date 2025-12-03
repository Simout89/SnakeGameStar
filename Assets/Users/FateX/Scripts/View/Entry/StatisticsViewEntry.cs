using TMPro;
using UnityEngine;

namespace Users.FateX.Scripts.View.Entry
{
    public class StatisticsViewEntry: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public void Init(string text)
        {
            _text.text = text;
        }
    }
}