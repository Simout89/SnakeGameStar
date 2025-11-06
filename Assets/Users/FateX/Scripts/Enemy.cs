using UnityEngine;

namespace Users.FateX.Scripts
{
    public class Enemy: MonoBehaviour
    {
        public void Move(Vector3 vector3)
        {
            transform.position = transform.position + vector3;
        }
    }
}