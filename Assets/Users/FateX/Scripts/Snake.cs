using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

public class Snake : MonoBehaviour
{
    [Inject] private IInputService _inputService;
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int startSize = 3;
    [SerializeField] private float segmentDistance = 0.5f;
    
    [Header("References")]
    [SerializeField] private GameObject segmentPrefab;
    
    private List<Transform> segments = new List<Transform>();
    public List<Transform> Segments => segments;

    private void Awake()
    {
        segments.Add(transform);
        for (int i = 0; i < startSize; i++)
        {
            Grow();
        }
    }

    private void Update()
    {
        float horizontal = _inputService.InputSystemActions.Player.Move.ReadValue<Vector2>().x;
        
        transform.Rotate(Vector3.forward * -horizontal * rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
        
        for (int i = 1; i < segments.Count; i++)
        {
            Transform prev = segments[i - 1];
            Transform curr = segments[i];

            float distance = Vector3.Distance(prev.position, curr.position);
            
            if(distance > segmentDistance)
            {
                Vector3 direction = (prev.position - curr.position).normalized;
                curr.position += direction * (distance - segmentDistance);
            }
            curr.rotation = Quaternion.Lerp(curr.rotation, prev.rotation, 0.5f);
        }
    }

    public void Grow()
    {
        GameObject newSegment = Instantiate(segmentPrefab);
        Transform last = segments[segments.Count - 1];
        newSegment.transform.position = last.position;
        segments.Add(newSegment.transform);
    }
    
#if UNITY_EDITOR
    [ContextMenu("GrowDebug")]
    public void GrowDebug()
    {
        for (int i = 0; i < 50; i++)
        {
            Grow();
        }
    }
#endif

}
