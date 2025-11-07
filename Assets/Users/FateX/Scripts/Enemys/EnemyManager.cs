using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Enemy;

public class EnemyManager : MonoBehaviour
{
    private Snake _snake;
    private float enemySpeed = 100f;
    
    private List<EnemyBase> _enemies = new List<EnemyBase>();

    public void SetSnake(Snake snake)
    {
        _snake = snake;
    }
    
    public void AddEnemy(EnemyBase enemyBase)
    {
        _enemies.Add(enemyBase);
    }

    public void FixedUpdate()
    {
        if (_snake == null || _snake.Segments.Count == 0) return;
        
        for (int i = 0; i < _enemies.Count; i++)
        {
            Transform nearestSegment = _snake.Segments[0];
            
            
            for (int j = 0; j < _snake.Segments.Count; j++)
            {
                if (Vector3.Distance(_snake.Segments[j].position, _enemies[i].transform.position) <
                    Vector3.Distance(nearestSegment.position, _enemies[i].transform.position))
                {
                    nearestSegment = _snake.Segments[j];
                }
            }
            
            _enemies[i].Move((nearestSegment.position - _enemies[i].transform.position).normalized * (Time.deltaTime * enemySpeed));
        }
    }
}
