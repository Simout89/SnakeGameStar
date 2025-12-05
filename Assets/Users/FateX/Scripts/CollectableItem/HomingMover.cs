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
        var callbacksToInvoke = new List<Action>();

        while (_active.Count > 0)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            toRemove.Clear();
            callbacksToInvoke.Clear();

            // Создаем копию ключей для безопасной итерации
            var keys = new List<Transform>(_active.Keys);

            foreach (var key in keys)
            {
                // Проверяем, что элемент еще существует (мог быть удален в callback)
                if (!_active.TryGetValue(key, out var data))
                    continue;

                if (key == null || data.Target == null)
                {
                    toRemove.Add(key);
                    continue;
                }

                key.position = Vector3.MoveTowards(
                    key.position, 
                    data.Target.position, 
                    data.Speed * Time.fixedDeltaTime
                );

                if (Vector3.Distance(key.position, data.Target.position) < 0.2f)
                {
                    toRemove.Add(key);
                    
                    if (data.OnReached != null)
                    {
                        callbacksToInvoke.Add(data.OnReached);
                    }
                }
            }

            // Сначала удаляем элементы
            foreach (var key in toRemove)
            {
                _active.Remove(key);
            }

            // Затем вызываем callback'и (после модификации словаря)
            foreach (var callback in callbacksToInvoke)
            {
                try
                {
                    callback.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка в OnReached callback: {e}");
                }
            }
        }

        _tickLoopRunning = false;
    }
}