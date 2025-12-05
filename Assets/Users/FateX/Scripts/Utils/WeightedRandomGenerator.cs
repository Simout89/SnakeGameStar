namespace Users.FateX.Scripts.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class WeightedRandomGenerator
    {
        private Dictionary<int, float> weights;
        private float weightIncrement;
        private System.Random random;

        /// <summary>
        /// Создает генератор случайных чисел с динамическими весами
        /// </summary>
        /// <param name="minValue">Минимальное значение (включительно)</param>
        /// <param name="maxValue">Максимальное значение (включительно)</param>
        /// <param name="weightIncrement">На сколько увеличивается вес при каждом невыпадении</param>
        public WeightedRandomGenerator(int minValue, int maxValue, float weightIncrement = 1f)
        {
            this.weightIncrement = weightIncrement;
            this.random = new System.Random();
            this.weights = new Dictionary<int, float>();

            // Инициализируем все числа с начальным весом 1
            for (int i = minValue; i <= maxValue; i++)
            {
                weights[i] = 1f;
            }
        }

        /// <summary>
        /// Генерирует случайное число с учетом весов
        /// </summary>
        public int Next()
        {
            // Вычисляем общий вес
            float totalWeight = weights.Values.Sum();

            // Генерируем случайное значение
            float randomValue = (float)(random.NextDouble() * totalWeight);

            // Выбираем число на основе весов
            float cumulative = 0f;
            int selectedNumber = weights.Keys.First();

            foreach (var kvp in weights)
            {
                cumulative += kvp.Value;
                if (randomValue <= cumulative)
                {
                    selectedNumber = kvp.Key;
                    break;
                }
            }

            // Обновляем веса: выпавшее число сбрасывается, остальные увеличиваются
            UpdateWeights(selectedNumber);

            return selectedNumber;
        }

        /// <summary>
        /// Обновляет веса после выпадения числа
        /// </summary>
        private void UpdateWeights(int selectedNumber)
        {
            // Сбрасываем вес выпавшего числа
            weights[selectedNumber] = 1f;

            // Увеличиваем вес всех остальных чисел
            var keys = weights.Keys.ToList();
            foreach (int key in keys)
            {
                if (key != selectedNumber)
                {
                    weights[key] += weightIncrement;
                }
            }
        }

        /// <summary>
        /// Сбрасывает все веса к начальным значениям
        /// </summary>
        public void Reset()
        {
            var keys = weights.Keys.ToList();
            foreach (int key in keys)
            {
                weights[key] = 1f;
            }
        }

        /// <summary>
        /// Получает текущий вес указанного числа
        /// </summary>
        public float GetWeight(int number)
        {
            return weights.ContainsKey(number) ? weights[number] : 0f;
        }

        /// <summary>
        /// Выводит текущие веса всех чисел (для отладки)
        /// </summary>
        public void PrintWeights()
        {
            Debug.Log("Текущие веса:");
            foreach (var kvp in weights.OrderBy(x => x.Key))
            {
                Debug.Log($"Число {kvp.Key}: вес {kvp.Value:F2}");
            }
        }
    }
}