using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;
using Zenject;
using Скриптерсы.Services;

public class SnakeController : MonoBehaviour
{
    private IInputService _inputService;
    
    private float rotationSpeed => SnakeData.BaseRotateSpeed;
    private float speed => SnakeData.BaseMoveSpeed;
    private float segmentDistance => SnakeData.SegmentDistance;
    
    [Header("References")]
    [field: SerializeField] public SnakeData SnakeData { get; private set; }
    [SerializeField] private SnakeSegmentBase segmentPrefab;
    [SerializeField] private SnakeHealth snakeHealth;
    [SerializeField] private SnakeInteraction _snakeInteraction;
    [SerializeField] private Rigidbody2D _rigidbody;
    
    private List<SnakeSegmentBase> segmentsBase = new List<SnakeSegmentBase>();
    public IReadOnlyList<SnakeSegmentBase> SegmentsBase => segmentsBase;
    public PlayerStats PlayerStats { get; private set; }
    public void Init(IInputService inputService, PlayerStats playerStats)
    {
        _inputService = inputService;

        PlayerStats = playerStats;

        var snakeSegmentBase = GetComponent<SnakeSegmentBase>();
        snakeSegmentBase.Init(this);
        segmentsBase.Add(snakeSegmentBase);
        
        snakeHealth.Add(snakeSegmentBase);
        _snakeInteraction.Add(snakeSegmentBase);
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

        Vector2 joyStickInput = new Vector2(_inputService.joyStickInput.x, -_inputService.joyStickInput.y);
    
        float snakeAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedJoystick = new Vector2(
            joyStickInput.x * Mathf.Cos(snakeAngle) - joyStickInput.y * Mathf.Sin(snakeAngle),
            joyStickInput.x * Mathf.Sin(snakeAngle) + joyStickInput.y * Mathf.Cos(snakeAngle)
        );

        float totalHorizontal = horizontal + rotatedJoystick.x;

        float rotation = -totalHorizontal * rotationSpeed * Time.fixedDeltaTime;
        _rigidbody.MoveRotation(_rigidbody.rotation + rotation);

        _rigidbody.linearVelocity = transform.up * (speed + PlayerStats.MoveSpeed.Sum);

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
    
    public void Grow(SnakeSegmentBase snakeSegmentBase)
    {
        var newSegment = Instantiate(snakeSegmentBase);
        Transform last = segmentsBase[segmentsBase.Count - 1].Body;
        newSegment.AdditionalParts.position = last.position;
        newSegment.Body.position = last.position;
        newSegment.Init(this);
        segmentsBase.Add(newSegment); 
        snakeHealth.Add(newSegment);
        _snakeInteraction.Add(newSegment);
    }
}
