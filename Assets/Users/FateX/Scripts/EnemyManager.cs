using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Snake _snake;
    [SerializeField] private float enemySpeed = 5f;
    
    private List<Enemy> _enemies = new List<Enemy>();
    
    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void Update()
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
