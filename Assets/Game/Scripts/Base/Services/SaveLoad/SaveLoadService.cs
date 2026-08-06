using System;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Data;
using UnityEngine;

namespace Game.Scripts.Base.Services.SaveLoad
{
    public sealed class SaveLoadService : ISaveLoadService
    {
        private readonly ISaveDataHandler _saveDataHandler;

        private string _prevJson;
        private bool _enabled;

        public SaveLoadService(ISaveDataHandler saveDataHandler)
        {
            _saveDataHandler = saveDataHandler;
        }

        public void Init()
        {
            _enabled = true;
        }

        public void DeInit()
        {
            _enabled = false;
        }

        public void SaveData(Action onComplete)
        {
            if ( !_enabled )
            {
                return;
            }

            foreach (var saveWriter in _saveDataHandler.SaveWriters)
            {
                saveWriter.WriteSave(_saveDataHandler.SaveData);
            }

            if ( _saveDataHandler.SaveData == null )
            {
                onComplete?.Invoke();
                return;
            }

            var json = JsonUtility.ToJson(_saveDataHandler.SaveData);
            if ( _prevJson == json )
            {
                onComplete?.Invoke();
                return;
            }
            _prevJson = json;
            
            PlayerPrefs.SetString("save_data", json);
            onComplete?.Invoke();
        }

        public void LoadData(Action<SaveData> onComplete)
        {
            var json = PlayerPrefs.GetString("save_data");

            _prevJson = json;

            var saveData = JsonUtility.FromJson<SaveData>(json);
            onComplete?.Invoke(saveData);
        }

        public void ResetSave()
        {
            if ( string.IsNullOrEmpty(_prevJson) )
            {
                return;
            }

            var saveData = JsonUtility.FromJson<SaveData>(_prevJson);
            _saveDataHandler.SaveData = saveData;
        }
    }
}