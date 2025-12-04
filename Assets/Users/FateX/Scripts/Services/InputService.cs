using System;
using UnityEngine;

namespace Users.FateX.Scripts.Services
{
    public class InputService: IInputService, IDisposable
    {
        public InputSystem_Actions InputSystemActions { get; private set; }
        
        public Vector2 joyStickInput { get; set; }

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
        public Vector2 joyStickInput { get; set; }
    }
}