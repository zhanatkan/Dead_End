using System;
using Game.Scripts.Settings;
using UnityEngine;
using PlayerSettings = Game.Scripts.Settings.CharacterSettings.PlayerSettings;

namespace Game.Scripts.Base.Services.Settings
{
    public interface ISettingsProvider
    {
        AudioSetting AudioSetting { get; }
        PlayerSettings PlayerSettings { get; }
        
        void LoadSettings(Action onComplete);
    }
}