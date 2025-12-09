using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class LairView : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionField;
        [SerializeField] private LairPage[] _lairPages;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;

        private LairPage currentPage;
        private bool isAnimating = false;

        private Sequence _sequence;

        private void Awake()
        {
            foreach (var lairPage in _lairPages)
            {
                LairPage page = lairPage;
                lairPage.Button.onClick.AddListener(() => HandleClick(page));
            }

            currentPage = _lairPages[0];
        }

        private void HandleClick(LairPage page)
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Select);
            
            if (currentPage == page || isAnimating)
                return;
            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Swipe);
            
            isAnimating = true;

            // Убиваем все активные твины на обеих страницах
            currentPage.RectTransform.DOKill();
            page.RectTransform.DOKill();

            // Устанавливаем начальную позицию для входящей страницы
            page.RectTransform.anchoredPosition = new Vector2(-3000, 0);
            page.Page.SetActive(true);
            
            page.Selected.SetActive(true);
            currentPage.Selected.SetActive(false);

            _sequence?.Kill();

            _sequence = DOTween.Sequence();

            _sequence.Append(currentPage.RectTransform.DOAnchorPos(new Vector2(3000, 0), 0.2f));
            _sequence.Append(page.RectTransform.DOAnchorPos(new Vector2(0, 0), 0.2f));

            LairPage previousPage = currentPage;
            
            _sequence.OnComplete(() =>
            {
                previousPage.Page.SetActive(false);
                // Сбрасываем позицию предыдущей страницы на случай возврата к ней
                previousPage.RectTransform.anchoredPosition = Vector2.zero;
                
                currentPage = page;
                isAnimating = false;
            });
        }

        public void SetDescription(string text)
        {
            descriptionField.text = text;
        }
        
        // Добавьте этот метод для очистки при уничтожении
        private void OnDestroy()
        {
            _sequence?.Kill();
            
            foreach (var lairPage in _lairPages)
            {
                lairPage.RectTransform.DOKill();
            }
        }
    }

    [Serializable]
    public class LairPage
    {
        public Button Button;
        public GameObject Page;
        public RectTransform RectTransform;
        public GameObject Selected;
    }
}