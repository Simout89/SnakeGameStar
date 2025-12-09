using Unity.VisualScripting;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Users.FateX.Scripts
{
    public class MainMenuSceneEntryPoint: IInitializable
    {
        public void Initialize()
        {
            Debug.Log(321);
            
            Time.timeScale = 1;
        }
    }
}