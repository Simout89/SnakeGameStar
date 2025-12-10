using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class SettingsView: MonoBehaviour
    {
        [Inject] private SettingsController SettingsController;

        [SerializeField] private Slider GlobalVolume;
        [SerializeField] private Slider MusicVolume;
        [SerializeField] private Slider EffectsVolume;

        [SerializeField] private Toggle ShowFps;
        [SerializeField] private TMP_Dropdown TargetFps;

        private void Awake()
        {
            GlobalVolume.value = SettingsController.SettingsSaveData.GlobalVolume;
            MusicVolume.value = SettingsController.SettingsSaveData.MusicVolume;
            EffectsVolume.value = SettingsController.SettingsSaveData.EffectVolume;

            ShowFps.isOn = SettingsController.SettingsSaveData.ShowFps;
            
            GlobalVolume.onValueChanged.AddListener(SettingsController.ChangeGlobalVolume);
            MusicVolume.onValueChanged.AddListener(SettingsController.ChangeMusicVolume);
            EffectsVolume.onValueChanged.AddListener(SettingsController.ChangeEffectVolume);
            
            ShowFps.onValueChanged.AddListener(SettingsController.ChangedShowFps);
            
            int[] fpsValues = {-1, 20, 30, 40, 60, 144 };
            
            TargetFps.ClearOptions();
            
            List<string> options = new List<string>();
            foreach (int fps in fpsValues)
                options.Add(fps.ToString());

            TargetFps.AddOptions(options);

            for (int i = 0; i < fpsValues.Length; i++)
            {
                if (fpsValues[i] == SettingsController.SettingsSaveData.CurrentFps)
                {
                    TargetFps.value = i;
                    break;
                }
            }
            
            TargetFps.onValueChanged.AddListener((value) => {SettingsController.ChangeFps(fpsValues[value]);});
        }
    }
}