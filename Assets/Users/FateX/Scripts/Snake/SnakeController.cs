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
    [SerializeField] private Rigidbody2D _rigidbody;
    
    private List<SnakeSegmentBase> segmentsBase = new List<SnakeSegmentBase>();
    public List<SnakeSegmentBase> SegmentsBase => segmentsBase;
    public void Init(IInputService inputService)
    {
        _inputService = inputService;
        
        segmentsBase.Add(GetComponent<SnakeSegmentBase>());
        
        for (int i = 0; i < startSize; i++)
        {
            Grow();
        }
    }

    private void FixedUpdate()
    {
        foreach (var segment in segmentsBase)
            segment.Tick();

        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = _inputService.InputSystemActions.Player.Move.ReadValue<Vector2>().x;

        Vector2 joyStickInput = new Vector2(SimpleInput.GetAxisRaw("Horizontal"), -SimpleInput.GetAxisRaw("Vertical"));
        
        float snakeAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedJoystick = new Vector2(
            joyStickInput.x * Mathf.Cos(snakeAngle) - joyStickInput.y * Mathf.Sin(snakeAngle),
            joyStickInput.x * Mathf.Sin(snakeAngle) + joyStickInput.y * Mathf.Cos(snakeAngle)
        );

        float totalHorizontal = horizontal + rotatedJoystick.x;

        float rotation = -totalHorizontal * rotationSpeed * Time.fixedDeltaTime;
        _rigidbody.MoveRotation(_rigidbody.rotation + rotation);

        Vector2 moveDirection = transform.up * speed;
        _rigidbody.MovePosition(_rigidbody.position + moveDirection * Time.fixedDeltaTime);

        UpdateSegments();
    }
    
    private void UpdateSegments()
    {
        for (int i = 1; i < segmentsBase.Count; i++)
        {
            Transform prev = segmentsBase[i - 1].Body;
            Transform curr = segmentsBase[i].Body;

            Vector3 targetPosition = prev.position - prev.up * segmentDistance;
            curr.position = Vector3.Lerp(curr.position, targetPosition, Time.deltaTime * 10f);
            if(segmentsBase[i].AdditionalParts != null)
                segmentsBase[i].AdditionalParts.position = Vector3.Lerp(curr.position, targetPosition, Time.deltaTime * 10f);

            Vector3 direction = prev.position - curr.position;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                curr.rotation = Quaternion.Lerp(curr.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
            }
        }
    }

    public void Grow()
    {
        var newSegment = Instantiate(segmentPrefab);
        Transform last = segmentsBase[segmentsBase.Count - 1].Body;
        newSegment.AdditionalParts.position = last.position;
        newSegment.Body.position = last.position;
        newSegment.Init();
        segmentsBase.Add(newSegment);
    }
    
    public void Grow(SnakeSegmentBase snakeSegmentBase)
    {
        var newSegment = Instantiate(snakeSegmentBase);
        Transform last = segmentsBase[segmentsBase.Count - 1].Body;
        newSegment.AdditionalParts.position = last.position;
        newSegment.Body.position = last.position;
        newSegment.Init();
        segmentsBase.Add(newSegment);
    }
}
