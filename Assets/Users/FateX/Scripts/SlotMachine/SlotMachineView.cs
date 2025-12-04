using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;
using System.Collections.Generic;
using System;
using DG.Tweening;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.SlotMachine
{
    public class SlotMachineView : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private Transform[] slotColumns; // 3 колонки для слотов
        [SerializeField] private GameObject slotItemPrefab; // Префаб элемента слота с Image компонентом
        [SerializeField] private int visibleItemsPerColumn = 5; // Количество видимых элементов в колонке
        [SerializeField] private float itemHeight = 100f; // Высота одного элемента
        [SerializeField] private Sprite[] randomSprites; // Случайные спрайты для вращения
        [SerializeField] private GameObject[] lights; // Массив лампочек для анимации
        [SerializeField] private float lightAnimationSpeed = 0.1f; // Скорость переключения лампочек
        [SerializeField] private Button button;

        [Inject] private GameConfig _gameConfig;

        private List<SlotColumn> columns = new List<SlotColumn>();
        private bool skipAnimation = false;
        private SlotMachinePrizeData currentPrize;
        private bool isLightAnimationRunning = false;

        public event Action OnAnimationCompleted;

        private void Awake()
        {
            InitializeColumns();

            // Выключаем все лампочки в начале
            if (lights != null)
            {
                foreach (GameObject light in lights)
                {
                    if (light != null)
                        light.SetActive(false);
                }
            }
        }

        private void InitializeColumns()
        {
            randomSprites = new Sprite[_gameConfig.GameConfigData.SlotMachinePrizeDatas.Length];

            for (int i = 0; i < _gameConfig.GameConfigData.SlotMachinePrizeDatas.Length; i++)
            {
                randomSprites[i] = _gameConfig.GameConfigData.SlotMachinePrizeDatas[i].Icon;
            }

            foreach (Transform columnTransform in slotColumns)
            {
                SlotColumn column = new SlotColumn();
                column.transform = columnTransform;
                column.items = new List<Image>();

                // Создаем больше элементов чем видимых для эффекта бесконечной прокрутки
                for (int i = 0; i < visibleItemsPerColumn + 2; i++)
                {
                    GameObject item = Instantiate(slotItemPrefab, columnTransform);
                    Image image = item.GetComponent<Image>();
                    column.items.Add(image);

                    // Позиционируем элементы
                    RectTransform rect = item.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0, -i * itemHeight);
                }

                columns.Add(column);
            }
        }

        public void StartGambling(SlotMachinePrizeData slotMachinePrizeData)
        {
            body.transform.localScale = Vector3.zero;
            body.SetActive(true);
            body.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true).OnComplete(() => {});
            skipAnimation = false;
            currentPrize = slotMachinePrizeData;
            StartSpin(slotMachinePrizeData).Forget();
        }

        public void SkipAnimation()
        {
            skipAnimation = true;
        }

        private async UniTask StartSpin(SlotMachinePrizeData prize)
        {
            // Запускаем анимацию лампочек
            StartLightAnimation().Forget();

            // Запускаем вращение всех колонок одновременно
            // Один и тот же приз выпадает во всех трёх слотах
            List<UniTask> spinTasks = new List<UniTask>();

            for (int i = 0; i < columns.Count; i++)
            {
                float stopDelay = i * 0.5f; // Каждая колонка останавливается с задержкой
                spinTasks.Add(SpinColumn(columns[i], prize.Icon, stopDelay));
            }

            await UniTask.WhenAll(spinTasks);

            // Останавливаем анимацию лампочек
            isLightAnimationRunning = false;

            // Финальная анимация победы лампочек
            await WinLightAnimation();

            await button.OnClickAsync();

            // Выключаем все лампочки
            TurnOffAllLights();

            body.SetActive(false);

            OnAnimationCompleted?.Invoke();
        }

        private async UniTask StartLightAnimation()
        {
            if (lights == null || lights.Length == 0) return;

            isLightAnimationRunning = true;
            int currentIndex = 0;

            while (isLightAnimationRunning)
            {
                // Выключаем все лампочки
                TurnOffAllLights();

                // Включаем текущую лампочку
                if (lights[currentIndex] != null)
                    lights[currentIndex].SetActive(true);

                // Переходим к следующей лампочке
                currentIndex = (currentIndex + 1) % lights.Length;

                // Задержка (с учетом скипа)
                float delay = skipAnimation ? lightAnimationSpeed * 0.2f : lightAnimationSpeed;
                await UniTask.Delay((int)(delay * 1000), ignoreTimeScale: true);
            }
        }

        private async UniTask WinLightAnimation()
        {
            if (lights == null || lights.Length == 0) return;

            // Быстрое мигание всех лампочек
            for (int blink = 0; blink < 5; blink++)
            {
                // Все включены
                foreach (GameObject light in lights)
                {
                    if (light != null)
                        light.SetActive(true);
                }

                await UniTask.Delay(100, ignoreTimeScale: true);

                // Все выключены
                TurnOffAllLights();

                await UniTask.Delay(100, ignoreTimeScale: true);
            }

            // Оставляем все включенными
            foreach (GameObject light in lights)
            {
                if (light != null)
                    light.SetActive(true);
            }
        }

        private void TurnOffAllLights()
        {
            if (lights == null) return;

            foreach (GameObject light in lights)
            {
                if (light != null)
                    light.SetActive(false);
            }
        }

        private async UniTask SpinColumn(SlotColumn column, Sprite targetSprite, float stopDelay)
        {
            float spinDuration = 2f; // Общая длительность вращения (без учета задержки)
            float fastSpinTime = 1.3f; // Время быстрого вращения
            float slowDownTime = 0.7f; // Время замедления

            float currentSpeed = 0f;
            float maxSpeed = 2000f; // Максимальная скорость в пикселях/сек
            float position = 0f;

            // Устанавливаем случайные спрайты для начала
            foreach (Image item in column.items)
            {
                item.sprite = randomSprites[Random.Range(0, randomSprites.Length)];
            }

            // Если скип активирован сразу, пропускаем задержку
            if (!skipAnimation)
            {
                await UniTask.Delay((int)(stopDelay * 1000),
                    ignoreTimeScale: true); // Задержка перед остановкой этой колонки
            }

            float elapsedTime = 0f;

            while (elapsedTime < spinDuration && !skipAnimation)
            {
                float deltaTime = Time.unscaledDeltaTime; // Используем unscaledDeltaTime вместо deltaTime
                elapsedTime += deltaTime;

                // Рассчитываем скорость (ускорение и замедление)
                if (elapsedTime < fastSpinTime)
                {
                    // Ускорение
                    currentSpeed = Mathf.Lerp(0, maxSpeed, elapsedTime / fastSpinTime);
                }
                else
                {
                    // Замедление
                    float slowDownProgress = (elapsedTime - fastSpinTime) / slowDownTime;
                    currentSpeed = Mathf.Lerp(maxSpeed, 0, slowDownProgress);
                }

                // Двигаем элементы
                position += currentSpeed * deltaTime;

                for (int i = 0; i < column.items.Count; i++)
                {
                    RectTransform rect = column.items[i].GetComponent<RectTransform>();
                    float newY = -i * itemHeight + position;

                    // Зацикливание элементов
                    while (newY > itemHeight)
                    {
                        newY -= (column.items.Count) * itemHeight;

                        // Меняем спрайт на случайный при зацикливании
                        column.items[i].sprite = randomSprites[Random.Range(0, randomSprites.Length)];
                    }

                    rect.anchoredPosition = new Vector2(0, newY);
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            // Плавное позиционирование на целевой спрайт с анимацией
            await SmoothSnapToTarget(column, targetSprite);
        }

        private async UniTask SmoothSnapToTarget(SlotColumn column, Sprite targetSprite)
        {
            int centerIndex = visibleItemsPerColumn / 2;

            // Сначала быстро находим ближайший элемент к центральной позиции
            float centerY = -centerIndex * itemHeight;
            int closestIndex = 0;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < column.items.Count; i++)
            {
                RectTransform rect = column.items[i].GetComponent<RectTransform>();
                float distance = Mathf.Abs(rect.anchoredPosition.y - centerY);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            // Если скип активирован, мгновенно устанавливаем финальные позиции
            if (skipAnimation)
            {
                for (int i = 0; i < column.items.Count; i++)
                {
                    RectTransform rect = column.items[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0, -i * itemHeight);

                    if (i == centerIndex)
                    {
                        column.items[i].sprite = targetSprite;
                    }
                    else
                    {
                        column.items[i].sprite = randomSprites[Random.Range(0, randomSprites.Length)];
                    }
                }

                return;
            }

            // Плавно двигаем элементы на их финальные позиции
            float snapDuration = 0.15f;
            float elapsed = 0f;

            // Запоминаем начальные позиции
            Vector2[] startPositions = new Vector2[column.items.Count];
            for (int i = 0; i < column.items.Count; i++)
            {
                startPositions[i] = column.items[i].GetComponent<RectTransform>().anchoredPosition;
            }

            while (elapsed < snapDuration)
            {
                elapsed += Time.unscaledDeltaTime; // Используем unscaledDeltaTime
                float t = elapsed / snapDuration;
                t = Mathf.SmoothStep(0, 1, t); // Плавная интерполяция

                for (int i = 0; i < column.items.Count; i++)
                {
                    RectTransform rect = column.items[i].GetComponent<RectTransform>();
                    Vector2 targetPos = new Vector2(0, -i * itemHeight);
                    rect.anchoredPosition = Vector2.Lerp(startPositions[i], targetPos, t);
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            // Устанавливаем финальные позиции и спрайты
            for (int i = 0; i < column.items.Count; i++)
            {
                RectTransform rect = column.items[i].GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -i * itemHeight);

                if (i == centerIndex)
                {
                    column.items[i].sprite = targetSprite;
                }
                else
                {
                    column.items[i].sprite = randomSprites[Random.Range(0, randomSprites.Length)];
                }
            }
        }

        private class SlotColumn
        {
            public Transform transform;
            public List<Image> items;
        }
    }
}