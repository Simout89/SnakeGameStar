using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.Tutorial
{
    public class TutorialView: MonoBehaviour
    {
        [Inject] private TutorialController _tutorialController;
        [Inject] private GameStateManager _gameStateManager;
        
        [SerializeField] private Button resumeButton;

        [SerializeField] private TutorialWindow[] tutorialWindows;

        private TutorialWindow currentWindow;

        private void Awake()
        {
            resumeButton.onClick.AddListener(ResumeButtonClick);
        }

        private void HandleShowWindow(TutorialWindowType tutorialWindowType)
        {
            ShowWindow(tutorialWindowType);
            _gameStateManager.PushState(GameStates.Tutorial);

        }

        public void ShowWindow(TutorialWindowType tutorialWindowType)
        {
            foreach (var tutorialWindow in tutorialWindows)
            {
                if (tutorialWindow.TutorialWindowType == tutorialWindowType)
                {
                    tutorialWindow.Window.SetActive(true);
                    resumeButton.gameObject.SetActive(true);
                    currentWindow = tutorialWindow;
                    break;
                }
            }
        }

        public void ResumeButtonClick()
        {
            resumeButton.gameObject.SetActive(false);
            currentWindow.Window.SetActive(false);
            _gameStateManager.PopState();
        }
    }

    [Serializable]
    public class TutorialWindow
    {
        public TutorialWindowType TutorialWindowType;
        public GameObject Window;
    }

    public enum TutorialWindowType
    {
        Move,
        CardSelect,
        XpHealth,
        KillEnemy,
        DeathMenu
    }
}