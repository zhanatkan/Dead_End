#if UNITY_WEBGL && GAME_PUSH
using System;
using System.Collections;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Data;
using GamePush;
using UnityEngine;

namespace Game.Scripts.Base.Services.SaveLoad
{
    public sealed class GPSaveLoadService : ISaveLoadService
    {
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ICoroutineRunner _coroutineRunner;

        private string _prevJson;
        private bool _canSync;
        private DateTime _lastSaveTime;

        private Action _onSyncComplete;
        private bool _enabled;
        private Coroutine _cor;

        public GPSaveLoadService(ISaveDataHandler saveDataHandler,
            ICoroutineRunner coroutineRunner)
        {
            _saveDataHandler = saveDataHandler;
            _coroutineRunner = coroutineRunner;
        }

        public void Init()
        {
            _enabled = true;
            _lastSaveTime = DateTime.Now;
            _cor = _coroutineRunner.StartCoroutine(AutoSaveRoutine());

            GP_Player.OnSyncComplete += OnSyncComplete;
            GP_Player.OnSyncError += OnSyncError;
        }

        public void DeInit()
        {
            _enabled = false;

            if (_cor != null)
            {
                _coroutineRunner.StopCoroutine(_cor);
                _cor = null;
            }
            
            GP_Player.OnSyncComplete -= OnSyncComplete;
            GP_Player.OnSyncError -= OnSyncError;
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
            
            var currentRecords = _saveDataHandler.SaveData.PlayerSaveData.Records;
            GP_Player.Set("save_data", json);
            GP_Player.Set("score", currentRecords);

            _onSyncComplete = onComplete;
            
            Debug.Log("Save data to local");
            GP_Player.Sync();
            _lastSaveTime = DateTime.Now;
        }

        public void LoadData(Action<SaveData> onComplete)
        {
            var json = GP_Player.GetString("save_data");
            _prevJson = json;
            
            if ( string.IsNullOrEmpty(json) )
            {
                onComplete?.Invoke(null);
            }
            else
            {
                var saveData = JsonUtility.FromJson<SaveData>(json);
                onComplete?.Invoke(saveData);
            }
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

        private IEnumerator AutoSaveRoutine()
        {
            while (_enabled)
            {
                yield return new WaitForSeconds(60f);
                
                if (_enabled && (DateTime.Now - _lastSaveTime).TotalMinutes >= 1)
                {
                    SaveData(null);
                }
            }
        }

        private void OnSyncComplete()
        {
            _onSyncComplete?.Invoke();
        }

        private void OnSyncError()
        {
            Debug.LogError("Player sync error");
            _onSyncComplete?.Invoke();
        }
    }
}
#endif