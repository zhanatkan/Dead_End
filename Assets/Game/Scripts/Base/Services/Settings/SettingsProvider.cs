using System;
using Game.Scripts.Settings;
using Game.Scripts.Settings.CharacterSettings;
using Game.Scripts.Settings.Inventory.Game.Scripts.Configs.Game;
using UnityEngine;
using VContainer;
using PlayerSettings = Game.Scripts.Settings.CharacterSettings.PlayerSettings;

namespace Game.Scripts.Base.Services.Settings
{
    public sealed class SettingsProvider : ISettingsProvider
    {
        public AudioSetting AudioSetting { get; private set; }
        public AdvertisementSetting AdvertisementSetting { get; private set; }
        public PlayerSettings PlayerSettings { get; private set; }
        public ItemsSetting ItemsSetting { get; private set; }
        public LevelSettings LevelSettings { get; private set; }
        public BotsSetting BotsSetting { get; private set; }

        [Inject]
        public SettingsProvider()
        {
            
        }
        
        public void LoadSettings(Action onComplete)
        {
            AudioSetting = Resources.Load<AudioSetting>(SettingsPath.AudioSetting);
            AdvertisementSetting = Resources.Load<AdvertisementSetting>(SettingsPath.AdvertisementSetting);
            PlayerSettings = Resources.Load<PlayerSettings>(SettingsPath.PlayerSettings);
            ItemsSetting = Resources.Load<ItemsSetting>(SettingsPath.ItemsSettings);
            LevelSettings = Resources.Load<LevelSettings>(SettingsPath.LevelSettings);
            BotsSetting = Resources.Load<BotsSetting>(SettingsPath.BotsSetting);
            
            onComplete?.Invoke();
        }
    }
}