using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Users.FateX.Scripts.View.Entry
{
    public class StatisticsViewEntry: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] public Image BackGround;
        public void Init(string text)
        {
            _text.text = text;
        }
    }
}