using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class SettingsController: IInitializable, IDisposable
    {
        [Inject] private ISaveLoadService _saveLoadService;
        private SettingsSaveData _settingsSaveData;

        public SettingsSaveData SettingsSaveData 
        { 
            get 
            { 
                return _settingsSaveData; 
            }
            set
            {
                _settingsSaveData = value;
                _saveLoadService.SaveSettings(_settingsSaveData);
            }
        }
        public event Action<SettingsSaveData> OnSettingsChanged;
        
        public void Initialize()
        {
            _settingsSaveData = _saveLoadService.LoadSettings();
            
            Application.targetFrameRate = _settingsSaveData.CurrentFps;
            
            AkUnitySoundEngine.SetRTPCValue("Main_Volume", _settingsSaveData.GlobalVolume);
            AkUnitySoundEngine.SetRTPCValue("Effects", _settingsSaveData.EffectVolume);
            AkUnitySoundEngine.SetRTPCValue("Music_Volume", _settingsSaveData.MusicVolume);
            
            LocalizationSettings.InitializationOperation.Completed += (op) =>
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[_settingsSaveData.Language];
                LocalizationSettings.SelectedLocale = locale;
            };
        }

        public void ChangedShowFps(bool value)
        {
            _settingsSaveData.ShowFps = value;
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }

        public void SaveSettings()
        {
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }

        public void ChangeGlobalVolume(float value)
        {
            _settingsSaveData.GlobalVolume = value;
            AkUnitySoundEngine.SetRTPCValue("Main_Volume", value);
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }
        
        public void ChangeEffectVolume(float value)
        {
            _settingsSaveData.EffectVolume = value;
            AkUnitySoundEngine.SetRTPCValue("Effects", value);
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }
        
        public void ChangeMusicVolume(float value)
        {
            _settingsSaveData.MusicVolume = value;
            AkUnitySoundEngine.SetRTPCValue("Music_Volume", value);
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }

        public void ChangeFps(int value)
        {
            _settingsSaveData.CurrentFps = value;
            Application.targetFrameRate = value;
            OnSettingsChanged?.Invoke(_settingsSaveData);
            _saveLoadService.SaveSettings(_settingsSaveData);
        }

        public void ChangeLanguage(int index)
        {
            _settingsSaveData.Language = index;

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            SaveSettings();
        }

        public void Dispose()
        {
            _saveLoadService.SaveSettings(_settingsSaveData);
        }
    }

    public class SettingsSaveData
    {
        public bool ShowFps;
        public float GlobalVolume = 30;
        public float MusicVolume = 100;
        public float EffectVolume = 100;
        public int CurrentFps = 60;
        public int Language;
        public bool CardTutorial;
        public bool MoveTutorial;
        public bool KillTutorial;
        public bool DeathTutorial;
    }
}