using System;
using Game.Scripts.Settings;
using Game.Scripts.Settings.Inventory.Game.Scripts.Configs.Game;
using UnityEngine;
using PlayerSettings = Game.Scripts.Settings.CharacterSettings.PlayerSettings;

namespace Game.Scripts.Base.Services.Settings
{
    public interface ISettingsProvider
    {
        AudioSetting AudioSetting { get; }
        PlayerSettings PlayerSettings { get; }
        ItemsSetting ItemsSetting { get; }
        
        void LoadSettings(Action onComplete);
    }
}