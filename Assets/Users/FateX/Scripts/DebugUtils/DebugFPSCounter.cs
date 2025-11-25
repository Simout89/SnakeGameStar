using TMPro;
using UnityEngine;

namespace Users.FateX.Scripts.DebugUtils
{
    public class DebugFPSCounter: MonoBehaviour
    {
        [SerializeField] private TMP_Text fpsText;       // Ссылка на UI Text для отображения FPS
        private int frameSample = 30; // Количество кадров для усреднения

        private int frameCount = 0;
        private float deltaTimeSum = 0f;

        void Update()
        {
            deltaTimeSum += Time.unscaledDeltaTime;
            frameCount++;

            if (frameCount >= frameSample)
            {
                float fps = frameCount / deltaTimeSum;
                if (fpsText != null)
                    fpsText.text = $"FPS: {Mathf.Ceil(fps)}";

                frameCount = 0;
                deltaTimeSum = 0f;
            }
        }
    }
}