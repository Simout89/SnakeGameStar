using UnityEngine;
using UnityEngine.SceneManagement;

namespace Users.FateX.Scripts.View
{
    public class MainMenu: MonoBehaviour
    {
        public void OnPlayClick()
        {
            SceneManager.LoadScene((int)Scenes.Gameplay);
        }
    }
}