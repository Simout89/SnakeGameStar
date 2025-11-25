using TMPro;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Users.FateX.Scripts.Cards
{
    public class CardEntryView: MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        public void Init(CardData cardData)
        {
            _image.sprite = cardData.Sprite;
        }

        public void Init(CardData cardData, string text)
        {
            _image.sprite = cardData.Sprite;

            _text.text = text;
        }
    }
}