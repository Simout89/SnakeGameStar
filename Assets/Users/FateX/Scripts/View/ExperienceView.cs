using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class ExperienceView: MonoBehaviour
    {
        [Inject] private ExperienceSystem _experienceSystem;
        
        [SerializeField] private Image _image;

        private void OnEnable()
        {
            _experienceSystem.OnChangeXp += HandleChangeXp;
            _experienceSystem.OnGetLevel += HandleChangeXp;
        }
        
        private void OnDisable()
        {
            _experienceSystem.OnChangeXp -= HandleChangeXp;
            _experienceSystem.OnGetLevel -= HandleChangeXp;

        }

        private void HandleChangeXp()
        {
            _image.DOComplete();
            _image.DOFillAmount(_experienceSystem.CurrentXp / _experienceSystem.NextLevelXp, 0.2f);
        }
    }
}