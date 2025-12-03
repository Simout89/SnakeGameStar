using System;
using UnityEngine;

namespace Users.FateX.Scripts
    {
        public class GameStateManager
        {
            public GameStates CurrentState => currentState;
            private GameStates currentState = GameStates.Play;
            public GameStates PreviousState => previousState;
            private GameStates previousState;
            public event Action<GameStates> OnStateChanged;

            public GameStateManager()
            {
                ChangeState(GameStates.Play);
                Cursor.lockState = CursorLockMode.Locked;
            }

            public void ChangeState(GameStates newState)
            {
                if(newState == currentState)
                    return;

                switch (newState)
                {
                    case GameStates.Play:
                    {
                        Time.timeScale = 1;
                        Cursor.lockState = CursorLockMode.None;
                    }
                        break;
                    case GameStates.CardMenu:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                    }
                        break;
                    case GameStates.Death:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                    }
                        break;
                    case GameStates.Pause:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                    }
                        break;
                    case GameStates.Gambling:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                    }
                        break;
                }

                previousState = currentState;
                currentState = newState;
                
                OnStateChanged?.Invoke(currentState);
            }
        }

        public enum GameStates
        {
            Play, 
            CardMenu,
            Death,
            Pause,
            Gambling
        }
    }