using Unity.Cinemachine;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        
        public void SetTarget(Transform target)
        {
            _cinemachineCamera.Target.TrackingTarget = target;
        }
    }
}