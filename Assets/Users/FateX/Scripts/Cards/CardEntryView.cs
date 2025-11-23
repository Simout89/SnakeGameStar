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
        public void Init(CardData cardData)
        {
            _image.sprite = cardData.Sprite;
        }
    }
}