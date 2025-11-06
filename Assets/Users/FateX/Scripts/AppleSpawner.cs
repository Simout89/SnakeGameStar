using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField] private float sizeX;
    [SerializeField] private float sizeY;
    
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Apple applePrefab;

    private Apple _apple;

    private void Awake()
    {
        _apple = Instantiate(applePrefab);

        ReplaceApple();
        
        _apple.OnCollect += HandleCollect;
    }

    private void ReplaceApple()
    {
        _apple.transform.position = new Vector3(centerPoint.position.x + Random.Range(-sizeX/2, sizeX/2), centerPoint.position.y + Random.Range(-sizeY/2, sizeY/2), 0);
    }

    private void OnDisable()
    {
        _apple.OnCollect -= HandleCollect;
    }

    private void HandleCollect()
    {
        ReplaceApple();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawLine(centerPoint.position - Vector3.down * sizeY/2 + Vector3.right * sizeX/2, centerPoint.position - Vector3.down * sizeY/2 - Vector3.right * sizeX/2);
        Gizmos.DrawLine(centerPoint.position + Vector3.down * sizeY/2 + Vector3.right * sizeX/2, centerPoint.position + Vector3.down * sizeY/2 - Vector3.right * sizeX/2);
        
        Gizmos.DrawLine(centerPoint.position + Vector3.right * sizeX/2 + Vector3.up * sizeY/2, centerPoint.position + Vector3.right * sizeX/2 - Vector3.up * sizeY/2);
        Gizmos.DrawLine(centerPoint.position - Vector3.right * sizeX/2 + Vector3.up * sizeY/2, centerPoint.position - Vector3.right * sizeX/2 - Vector3.up * sizeY/2);
    }
}
