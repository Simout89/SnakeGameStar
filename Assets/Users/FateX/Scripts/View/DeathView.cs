using UnityEngine;
using UnityEngine.SceneManagement;

namespace Users.FateX.Scripts.View
{
    public class DeathView: MonoBehaviour
    {
        [SerializeField] private GameObject body;

        public void Show()
        {
            body.SetActive(true);
        }

        public void Hide()
        {
            body.SetActive(false);
        }

        public void OnRestartClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnMenuClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}