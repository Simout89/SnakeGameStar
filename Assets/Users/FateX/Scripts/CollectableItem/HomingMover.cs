using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HomingMover
{
    private static bool _tickLoopRunning;
    private static readonly Dictionary<Transform, HomingData> _active = new();

    private class HomingData
    {
        public Transform Target;
        public float Speed;
        public Action OnReached;
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        _active.Clear();
        _tickLoopRunning = false;
    }

    public static void StartMove(Transform from, Transform to, Action onReached = null, float speed = 15f)
    {
        _active[from] = new HomingData 
        { 
            Target = to, 
            Speed = speed,
            OnReached = onReached
        };

        if (!_tickLoopRunning)
        {
            _tickLoopRunning = true;
            GlobalTickLoop().Forget();
        }
    }

    private static async UniTask GlobalTickLoop()
    {
        var toRemove = new List<Transform>();

        while (_active.Count > 0)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            toRemove.Clear();

            foreach (var kvp in _active)
            {
                if (kvp.Key == null || kvp.Value.Target == null)
                {
                    toRemove.Add(kvp.Key);
                    continue;
                }

                kvp.Key.position = Vector3.MoveTowards(
                    kvp.Key.position, 
                    kvp.Value.Target.position, 
                    kvp.Value.Speed * Time.fixedDeltaTime
                );

                if (Vector3.Distance(kvp.Key.position, kvp.Value.Target.position) < 0.2f)
                {
                    toRemove.Add(kvp.Key);
                    
                    try
                    {
                        kvp.Value.OnReached?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Ошибка в OnReached callback: {e}");
                    }
                }
            }

            foreach (var key in toRemove)
            {
                _active.Remove(key);
            }
        }

        _tickLoopRunning = false;
    }
}