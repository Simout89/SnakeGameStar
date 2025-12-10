using UnityEngine;

namespace Users.FateX.Scripts
{
    public class MonoHelper : MonoBehaviour
    {
        public MonoBehaviour MonoBehaviour => this;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}