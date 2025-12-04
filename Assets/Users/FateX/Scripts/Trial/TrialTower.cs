using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Upgrade;
using Zenject;

namespace Users.FateX.Scripts.Trial
{
    public class TrialTower : MonoBehaviour
    {
        [Inject] private TrialDirector _trialDirector;
        
        [SerializeField] private TriggerDetector _triggerDetector;
        [SerializeField] private SpriteRenderer _captureField;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject captureSlider;

        private List<Collider2D> colliders = new List<Collider2D>();
        private CancellationTokenSource _cancellationTokenSource;
        private Vector3 originScaleSlider;
        private bool isTimerRunning = false;
        private float captureTime = 5;
        private bool captured = false;

        private void OnEnable()
        {
            _triggerDetector.onTriggerEntered += HandleTriggerEntered;
            _triggerDetector.onTriggerExited += HandleTriggerExited;
        }

        private void OnDisable()
        {
            _triggerDetector.onTriggerEntered -= HandleTriggerEntered;
            _triggerDetector.onTriggerExited -= HandleTriggerExited;
        }


        private void HandleTriggerEntered(Collider2D obj)
        {
            if (obj.GetComponent<SnakeSegmentBase>())
            {
                colliders.Add(obj);

                CheckOnSnake();
            }
        }

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            originScaleSlider = captureSlider.transform.localScale;
        }

        private void CheckOnSnake()
        {
            if (colliders.Count > 0)
            {
                Debug.Log("Змея есть");
                if (!isTimerRunning && !captured)
                {
                    WaitForFixedSeconds(captureTime, _cancellationTokenSource.Token).Forget();
                }
            }
            else
            {
                Debug.Log("Змея нет");
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource(); // Пересоздаем токен
                isTimerRunning = false;

                captureSlider.transform.DOComplete();
                captureSlider.transform.DOScale(Vector3.zero, 0.3f).OnComplete((() => captureSlider.SetActive(false)));

                _captureField.DOComplete();
                _captureField.DOFade(0, 0.3f);
                _image.fillAmount = 0f;
            }
        }

        private void HandleTriggerExited(Collider2D obj)
        {
            if (obj.GetComponent<SnakeSegmentBase>())
            {
                colliders.Remove(obj);

                CheckOnSnake();
            }
        }
        
        private async UniTask WaitForFixedSeconds(float seconds, CancellationToken cancellationToken = default)
        {
            isTimerRunning = true;
            captureSlider.transform.localScale = Vector3.zero;
            captureSlider.transform.DOComplete();
            captureSlider.transform.DOScale(originScaleSlider, 0.3f);
            _captureField.DOComplete();
            captureSlider.SetActive(true);
            _captureField.DOFade(0.2f, 0.3f);
            
            float elapsed = 0f;
        
            while (elapsed < seconds)
            {
                await UniTask.WaitForFixedUpdate(cancellationToken);
                _image.fillAmount = elapsed / seconds;
                elapsed += Time.fixedDeltaTime;
            }

            isTimerRunning = false;
            captureSlider.transform.DOComplete();
            captureSlider.transform.DOScale(Vector3.zero, 0.3f).OnComplete((() => captureSlider.SetActive(false)));
            _captureField.DOComplete();
            _captureField.DOFade(0, 0.3f);

            
            OnTimerEnd();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private void OnTimerEnd()
        {
            captured = true;
            _trialDirector.OnTowerCaptured(this);
        }
    }
}