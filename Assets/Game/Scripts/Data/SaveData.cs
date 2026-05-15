using System;

namespace Game.Scripts.Data
{
    [Serializable]
    public sealed class SaveData
    {
        public SettingsSaveData SettingsSaveData;
        
        public SaveData()
        {
            SettingsSaveData = new SettingsSaveData();
        }
    }
}