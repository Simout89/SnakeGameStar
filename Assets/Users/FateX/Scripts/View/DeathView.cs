using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Serialization;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Utils;
using Users.FateX.Scripts.View.Entry;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class DeathView : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private Transform container;
        [FormerlySerializedAs("containerAchievemnt")] [SerializeField] private Transform containerAchievement;
        [SerializeField] private Transform squaresContainer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private StatisticsViewEntry _statisticsViewEntry;
        [SerializeField] private int gridSize = 20;
        [SerializeField] private float animationDuration = 2f;
        [SerializeField] private Color squareColor = Color.black;
        [Inject] private AchievementController _achievementController;
        [Inject] private GameConfig _gameConfig;
        [Inject] private IStatisticsService _statisticsService;
        
        private List<Image> squares = new List<Image>();
        private CancellationTokenSource cancellationTokenSource;

        public event Action OnDeathAnimationEnd;

        public async void Show()
        {
            cancellationTokenSource = new CancellationTokenSource();
            
            await PlayDeathAnimation(cancellationTokenSource.Token);
            
            OnDeathAnimationEnd?.Invoke();
            
            body.SetActive(true);

            var statistics = _statisticsService.GetStatistics();
                
            for (int i = 0; i < statistics.Length; i++)
            {
                var entryText = Instantiate(_statisticsViewEntry, container);
                entryText.Init(statistics[i]);
                if (i % 2 == 0)
                {
                    entryText.BackGround.color = MyUtils.HexToColor("#272727");
                }
            }

            var achievements= _achievementController.GetNewObtainedAchievement();
            foreach (var achievement in achievements)
            {
                Instantiate(_gameConfig.GameConfigData.DeathAchievementEntryView, containerAchievement).Init(achievement, _gameConfig);
            }
        }

        public void Hide()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            
            body.SetActive(false);
            ClearSquares();
        }

        private async UniTask PlayDeathAnimation(CancellationToken token)
        {
            CreateGrid();
            
            List<Vector2Int> spiralOrder = GetSpiralOrder();
            float delayPerSquare = animationDuration / spiralOrder.Count;

            foreach (var pos in spiralOrder)
            {
                if (token.IsCancellationRequested) return;
                
                int index = pos.y * gridSize + pos.x;
                if (index < squares.Count)
                {
                    squares[index].enabled = true;
                }
                await UniTask.Delay((int)(delayPerSquare * 1000), ignoreTimeScale: true, cancellationToken: token);
            }
        }

        private void CreateGrid()
        {
            ClearSquares();

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float rectWidth = canvasRect.rect.width / gridSize;
            float rectHeight = canvasRect.rect.height / gridSize;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    GameObject squareObj = new GameObject($"Square_{x}_{y}");
                    squareObj.transform.SetParent(squaresContainer);
                    
                    Image img = squareObj.AddComponent<Image>();
                    img.color = squareColor;
                    img.enabled = false;
                    
                    RectTransform rect = squareObj.GetComponent<RectTransform>();
                    rect.localScale = Vector3.one;
                    rect.sizeDelta = new Vector2(rectWidth, rectHeight);
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.pivot = new Vector2(0, 1);
                    rect.anchoredPosition = new Vector2(x * rectWidth, -y * rectHeight);
                    
                    squares.Add(img);
                }
            }
        }

        private void ClearSquares()
        {
            foreach (var square in squares)
            {
                if (square != null) Destroy(square.gameObject);
            }
            squares.Clear();
        }

        private List<Vector2Int> GetSpiralOrder()
        {
            List<Vector2Int> spiral = new List<Vector2Int>();
            int left = 0, right = gridSize - 1;
            int top = 0, bottom = gridSize - 1;

            while (left <= right && top <= bottom)
            {
                for (int x = left; x <= right; x++)
                    spiral.Add(new Vector2Int(x, top));
                top++;

                for (int y = top; y <= bottom; y++)
                    spiral.Add(new Vector2Int(right, y));
                right--;

                if (top <= bottom)
                {
                    for (int x = right; x >= left; x--)
                        spiral.Add(new Vector2Int(x, bottom));
                    bottom--;
                }

                if (left <= right)
                {
                    for (int y = bottom; y >= top; y--)
                        spiral.Add(new Vector2Int(left, y));
                    left++;
                }
            }

            return spiral;
        }

        public void OnRestartClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnMenuClick()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}