using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
    public class AuthorsLink: MonoBehaviour
    {
        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }
    }
}