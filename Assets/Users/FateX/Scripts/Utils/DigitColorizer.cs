using System.Text.RegularExpressions;
using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
    public static class DigitColorizer
    {
        /// <summary>
        /// Заменяет все цифры в тексте на цветной вариант.
        /// </summary>
        /// <param name="input">Исходный текст</param>
        /// <param name="color">Цвет в формате HEX или ColorUtility.ToHtmlStringRGB</param>
        /// <returns>Текст с окрашенными цифрами</returns>
        public static string ColorDigits(string input, string color)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Регулярка ищет все цифры
            return Regex.Replace(input, @"\d", match => $"<color=#{color}>{match.Value}</color>");
        }

        /// <summary>
        /// Перегрузка: можно передать UnityEngine.Color напрямую
        /// </summary>
        public static string ColorDigits(string input, Color color)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(color);
            return ColorDigits(input, hexColor);
        }
    }
}