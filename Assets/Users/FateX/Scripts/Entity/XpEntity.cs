using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Data.Enity;

namespace Users.FateX.Scripts.Entity
{
    public class XpEntity : Entity
    {
        [SerializeField] private XpEntityData _xpEntityData;
        [SerializeField] private SpriteRenderer _sprite;
        private float xpAmount;

        private void Awake()
        {
            if(_xpEntityData.Sprite != null)
                _sprite.sprite = _xpEntityData.Sprite;
            xpAmount = _xpEntityData.XpAmount;
        }
    }
}