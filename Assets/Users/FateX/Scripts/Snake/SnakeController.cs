using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Upgrade;
using Zenject;
using Скриптерсы.Services;

public class SnakeController : MonoBehaviour
{
    private IInputService _inputService;
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int startSize = 3;
    [SerializeField] private float segmentDistance = 0.5f;
    
    [Header("References")]
    [SerializeField] private SnakeSegmentBase segmentPrefab;
    [SerializeField] private SnakeHealth snakeHealth;
    
    private List<Transform> segments = new List<Transform>();
    private List<SnakeSegmentBase> segmentsBase = new List<SnakeSegmentBase>();
    public List<Transform> Segments => segments;
    public void Init(IInputService inputService)
    {
        _inputService = inputService;
        
        segments.Add(transform);
        for (int i = 0; i < startSize; i++)
        {
            Grow();
        }
    }

    private void FixedUpdate()
    {
        foreach (var segment in segmentsBase)
        {
            segment.Tick();
        }
    }

    private void Update()
    {
        float horizontal = _inputService.InputSystemActions.Player.Move.ReadValue<Vector2>().x;
        Vector2 joyStickInput = new Vector2(SimpleInput.GetAxisRaw("Horizontal"), -SimpleInput.GetAxisRaw("Vertical"));
        
        float snakeAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedJoystick = new Vector2(
            joyStickInput.x * Mathf.Cos(snakeAngle) - joyStickInput.y * Mathf.Sin(snakeAngle),
            joyStickInput.x * Mathf.Sin(snakeAngle) + joyStickInput.y * Mathf.Cos(snakeAngle)
        );
        
        float totalHorizontal = horizontal + rotatedJoystick.x;
        
        transform.Rotate(Vector3.forward * (-totalHorizontal) * rotationSpeed * Time.deltaTime);
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
        var newSegment = Instantiate(segmentPrefab);
        Transform last = segments[segments.Count - 1];
        newSegment.transform.position = last.position;
        segments.Add(newSegment.transform);
        newSegment.Init();
        segmentsBase.Add(newSegment);
    }
}
