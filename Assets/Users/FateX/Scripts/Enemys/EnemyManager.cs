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

    public void SetSnake(SnakeController snakeController)
    {
        _snakeController = snakeController;
    }
    
    public void AddEnemy(EnemyBase enemyBase)
    {
        _enemies.Add(enemyBase);
        enemyBase.OnDie += HandleEnemyDie;
    }

    private void HandleEnemyDie(EnemyBase enemyBase)
    {
        enemyBase.OnDie -= HandleEnemyDie;
        _enemies.Remove(enemyBase);
    }

    public void FixedUpdate()
    {
        if (_snakeController == null || _snakeController.Segments.Count == 0) return;
        
        for (int i = 0; i < _enemies.Count; i++)
        {
            Transform nearestSegment = _snakeController.Segments[0];
            
            
            for (int j = 0; j < _snakeController.Segments.Count; j++)
            {
                if (Vector3.Distance(_snakeController.Segments[j].position, _enemies[i].transform.position) <
                    Vector3.Distance(nearestSegment.position, _enemies[i].transform.position))
                {
                    nearestSegment = _snakeController.Segments[j];
                }
            }
            
            _enemies[i].Move((nearestSegment.position - _enemies[i].transform.position).normalized * (Time.deltaTime * enemySpeed));
        }
    }
}
