using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class WindowNavigator: MonoBehaviour
    {
        [SerializeField] private GameObject[] windows;
        [Inject] private IInputService _inputService;

        private void OnEnable()
        {
            _inputService.InputSystemActions.Player.Escape.performed += HandlePerformed;
        }

        private void OnDisable()
        {
            _inputService.InputSystemActions.Player.Escape.performed -= HandlePerformed;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            ReturnPreviousWindow();
        }

        private Stack<GameObject> screenStack = new();

        private void Awake()
        {
            foreach (var VARIABLE in windows)
            {
                VARIABLE.SetActive(false);
            }
            
            screenStack.Push(windows[0]);
            screenStack.Peek().SetActive(true);
        }

        public void ChangeWindow(GameObject newWindow)
        {
            screenStack.Peek().SetActive(false);

            screenStack.Push(newWindow);
            
            screenStack.Peek().SetActive(true);
        }

        public void ReturnPreviousWindow()
        {
            if(screenStack.Count <= 1)
                return;
            
            screenStack.Pop().SetActive(false);
            screenStack.Peek().SetActive(true);

        }
    }
}