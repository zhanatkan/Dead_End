using System;

namespace Game.Scripts.Data
{
    [Serializable]
    public sealed class SaveData
    {
        public SettingsSaveData SettingsSaveData;
        public PlayerSaveData PlayerSaveData;
        
        public SaveData()
        {
            SettingsSaveData = new SettingsSaveData();
            PlayerSaveData = new PlayerSaveData();
        }
    }
}