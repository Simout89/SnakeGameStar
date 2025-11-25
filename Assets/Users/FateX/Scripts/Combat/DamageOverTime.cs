using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public struct DamageOverTimeInfo
    {
        public IDamageable Damageable;
        public IDamageDealer DamageDealer;
        public float TickDelay;
        public float NextTickTime;
        public DamageInfo Damage;

        public DamageOverTimeInfo(IDamageable damageable, IDamageDealer damageDealer, float delay, DamageInfo dmg)
        {
            Damageable = damageable;
            DamageDealer = damageDealer;
            TickDelay = delay;
            NextTickTime = 0f;
            Damage = dmg;
        }
    }

    public static class DamageOverTime
    {
        private static readonly List<DamageOverTimeInfo> _active = new();
        private static bool _tickLoopRunning;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _active.Clear();
            _tickLoopRunning = false;
        }

        public static void StartDot(IDamageable target, IDamageDealer damageDealer, float tickDelay, DamageInfo damage)
        {
            foreach (var dot in _active)
            {
                if (dot.Damageable == target && dot.DamageDealer == damageDealer)
                    return;
            }

            _active.Add(new DamageOverTimeInfo(target, damageDealer, tickDelay, damage));

            if (!_tickLoopRunning)
            {
                _tickLoopRunning = true;
                GlobalTickLoop().Forget();
            }
        }

        public static void StopDot(IDamageable target, IDamageDealer damageDealer)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var dot = _active[i];
                if (dot.Damageable == target && dot.DamageDealer == damageDealer)
                    _active.RemoveAt(i);
            }
        }

        public static void StopAllDots(IDamageable target)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (_active[i].Damageable == target)
                    _active.RemoveAt(i);
            }
        }
        
        public static void StopAllDots(IDamageDealer damageDealer)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (_active[i].DamageDealer == damageDealer)
                    _active.RemoveAt(i);
            }
        }

        private static async UniTask GlobalTickLoop()
        {
            while (_active.Count > 0)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
                float time = Time.time;

                var activeCopy = new List<DamageOverTimeInfo>(_active);

                foreach (var dot in activeCopy)
                {
                    if (!_active.Contains(dot)) 
                        continue;

                    if (dot.Damageable == null)
                    {
                        _active.Remove(dot);
                        continue;
                    }

                    if (time >= dot.NextTickTime)
                    {
                        dot.Damageable.TakeDamage(dot.Damage);
                        GameEvents.DamageDealt(dot.Damage);

                        int index = _active.IndexOf(dot);
                        if (index != -1)
                        {
                            var updatedDot = _active[index];
                            updatedDot.NextTickTime = time + updatedDot.TickDelay;
                            _active[index] = updatedDot;
                        }
                    }
                }
            }

            _tickLoopRunning = false;
        }

    }
}