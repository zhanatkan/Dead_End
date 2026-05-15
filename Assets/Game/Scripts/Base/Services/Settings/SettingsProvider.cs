using System;
using Game.Scripts.Settings;
using Game.Scripts.Settings.CharacterSettings;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.Settings
{
    public sealed class SettingsProvider : ISettingsProvider
    {
        public AudioSetting AudioSetting { get; private set; }
        public CharacterMoveSetting CharacterMoveSetting { get; private set; }

        [Inject]
        public SettingsProvider()
        {
            
        }
        
        public void LoadSettings(Action onComplete)
        {
            AudioSetting = Resources.Load<AudioSetting>(SettingsPath.AudioSetting);
            CharacterMoveSetting = Resources.Load<CharacterMoveSetting>(SettingsPath.CharacterMoveSetting);
            
            onComplete?.Invoke();
        }
    }
}