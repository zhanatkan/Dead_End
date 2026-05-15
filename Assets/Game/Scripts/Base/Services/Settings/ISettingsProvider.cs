using System;
using Game.Scripts.Settings;
using Game.Scripts.Settings.CharacterSettings;
using UnityEngine;

namespace Game.Scripts.Base.Services.Settings
{
    public interface ISettingsProvider
    {
        AudioSetting AudioSetting { get; }
        CharacterMoveSetting CharacterMoveSetting { get; }
        
        void LoadSettings(Action onComplete);
    }
}