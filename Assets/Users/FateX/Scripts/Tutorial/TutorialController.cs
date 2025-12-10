using System;
using Zenject;

namespace Users.FateX.Scripts.Tutorial
{
    public class TutorialController: IInitializable
    {
        public event Action<TutorialWindowType> OnShowWindow;
        
        public void Initialize()
        {
            
        }

        public void OnResumeButtonClicked()
        {
            
        }
    }
}