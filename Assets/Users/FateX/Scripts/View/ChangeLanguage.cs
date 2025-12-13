using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class ChangeLanguage: MonoBehaviour
    {
        [Inject] private SettingsController _settingsController;
        [SerializeField] private TMP_Dropdown _dropdown;
        
        
        private void Awake()
        {
            _dropdown.ClearOptions();
            
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            }
            
            _dropdown.RefreshShownValue();
            
            _dropdown.onValueChanged.AddListener(OnLanguageChanged);

        }
        
        private void OnLanguageChanged(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}