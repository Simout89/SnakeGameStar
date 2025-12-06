using System;
using System.Collections.Generic;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class GameStateManager
    {
        public GameStates CurrentState => stateStack.Count > 0 ? stateStack.Peek() : GameStates.Play;
        private Stack<GameStates> stateStack = new Stack<GameStates>();
        public event Action<GameStates> OnStateChanged;

        public GameStateManager()
        {
            PushState(GameStates.Play);
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Добавить новое состояние в стек (поставить игру на паузу)
        public void PushState(GameStates newState)
        {
            // Не добавляем дубликаты подряд
            if (stateStack.Count > 0 && stateStack.Peek() == newState)
                return;

            stateStack.Push(newState);
            ApplyState(newState);
            OnStateChanged?.Invoke(newState);
        }

        // Убрать текущее состояние из стека (вернуться к предыдущему)
        public void PopState()
        {
            if (stateStack.Count <= 1)
            {
                Debug.LogWarning("Нельзя удалить последнее состояние из стека!");
                return;
            }

            stateStack.Pop();
            GameStates previousState = stateStack.Peek();
            ApplyState(previousState);
            OnStateChanged?.Invoke(previousState);
        }

        // Убрать все состояния до определенного
        public void PopToState(GameStates targetState)
        {
            while (stateStack.Count > 1 && stateStack.Peek() != targetState)
            {
                stateStack.Pop();
            }

            if (stateStack.Count > 0)
            {
                ApplyState(stateStack.Peek());
                OnStateChanged?.Invoke(stateStack.Peek());
            }
        }

        // Очистить стек и установить новое состояние
        public void ClearAndSetState(GameStates newState)
        {
            stateStack.Clear();
            PushState(newState);
        }

        private void ApplyState(GameStates state)
        {
            switch (state)
            {
                case GameStates.Play:
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.None;
                }
                    break;

                case GameStates.CardMenu:
                case GameStates.Death:
                case GameStates.Pause:
                case GameStates.Tutorial:
                case GameStates.Gambling:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                }
                    break;
            }
        }

        // Проверить, находится ли состояние в стеке
        public bool IsStateInStack(GameStates state)
        {
            return stateStack.Contains(state);
        }

        // Получить количество состояний в стеке
        public int GetStackCount()
        {
            return stateStack.Count;
        }
    }

    public enum GameStates
    {
        Play,
        CardMenu,
        Death,
        Pause,
        Gambling,
        Tutorial
    }
}