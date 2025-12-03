using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Enemy;

public class EnemyManager : MonoBehaviour
{
    private SnakeController _snakeController;
    private float enemySpeed = 100f;
    
    private List<EnemyBase> _enemies = new List<EnemyBase>();
    public event Action<EnemyBase> OnEnemyDie;
    public event Action<int> AliveEnemyCountChanged;
    public event Action<EnemyBase> OnEnemyTakeDamage;

    public void SetSnake(SnakeController snakeController)
    {
        _snakeController = snakeController;
    }
    
    public void AddEnemy(EnemyBase enemyBase)
    {
        _enemies.Add(enemyBase);
        enemyBase.OnDie += HandleEnemyDie;
        enemyBase.OnEnemyTakeDamage += HandleEnemyTakeDamage;
        AliveEnemyCountChanged?.Invoke(_enemies.Count);
    }

    private void HandleEnemyDie(EnemyBase enemyBase)
    {
        enemyBase.OnDie -= HandleEnemyDie;
        enemyBase.OnEnemyTakeDamage -= HandleEnemyTakeDamage;

        OnEnemyDie?.Invoke(enemyBase);
        GameEvents.EnemyDie(enemyBase.GetData(), enemyBase.lastDamageInfo);
        _enemies.Remove(enemyBase);
        AliveEnemyCountChanged?.Invoke(_enemies.Count);
    }

    private void HandleEnemyTakeDamage(EnemyBase obj)
    {
        OnEnemyTakeDamage?.Invoke(obj);
    }

    public void FixedUpdate()
    {
        if (_snakeController == null || _snakeController.SegmentsBase.Count == 0) return;

        for (int i = 0; i < _enemies.Count; i++)
        {
            Transform nearestSegment = _snakeController.SegmentsBase[0].Body;
            float minDistance = Vector3.Distance(nearestSegment.position, _enemies[i].transform.position);

            for (int j = 1; j < _snakeController.SegmentsBase.Count; j++)
            {
                float dist = Vector3.Distance(_snakeController.SegmentsBase[j].Body.position, _enemies[i].transform.position);
                if (dist < minDistance)
                {
                    nearestSegment = _snakeController.SegmentsBase[j].Body;
                    minDistance = dist;
                }
            }

            Vector2 direction = ((Vector2)nearestSegment.position - (Vector2)_enemies[i].transform.position).normalized;

            if (Mathf.Abs(direction.x) > 0.01f)
            {
                Vector3 localScale = _enemies[i].transform.localScale;
                localScale.x = direction.x >= 0 ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
                _enemies[i].transform.localScale = localScale;
            }
            //Debug.Log($"Enemy {i}: distance to target={minDistance}, direction={direction}");

            _enemies[i].Move(direction);
        }
    }

}
