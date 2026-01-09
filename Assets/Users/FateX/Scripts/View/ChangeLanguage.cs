using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class ChangeLanguage: MonoBehaviour
    {
        [Inject] private SettingsController _settingsController;
        [SerializeField] private TMP_Dropdown _dropdown;
        
        
        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            _dropdown.ClearOptions();
            
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            }
            
            _dropdown.RefreshShownValue();
            _dropdown.value = _settingsController.SettingsSaveData.Language;
            _dropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
        
        private void OnLanguageChanged(int index)
        {
            _settingsController.ChangeLanguage(index);
        }
    }
}