using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Services
{
    public class InputService: IInputService, IDisposable
    {
        public InputSystem_Actions InputSystemActions { get; private set; }
        
        public InputService() 
        {
            InputSystemActions = new InputSystem_Actions();
            InputSystemActions.Enable();
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