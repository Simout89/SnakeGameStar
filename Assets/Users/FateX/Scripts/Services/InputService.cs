using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Services
{
    public class InputService: IInputService, IDisposable
    {
        public InputSystem_Actions InputSystemActions { get; private set; }
        
        public InputService() // Убираем IInitializable
        {
            InputSystemActions = new InputSystem_Actions();
            InputSystemActions.Enable();
            Debug.Log("1234 - InputService создан");
        }
        
        public void Dispose()
        {
            InputSystemActions.Disable();
        }

    }

    public interface IInputService
    {
        public InputSystem_Actions InputSystemActions { get;}
    }
}