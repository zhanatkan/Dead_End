#if UNITY_WEBGL && GAME_PUSH
using System;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using GamePush;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.Authorization
{
    public class GPAuthorizationService : IAuthorizationService
    {
        readonly ISaveLoadService _saveLoadService;
        readonly ISaveDataHandler _saveDataHandler;

        private Action _onLoginComplete;
        private Action _onLoginError;
        
        [Inject]
        public GPAuthorizationService(ISaveLoadService saveLoadService, ISaveDataHandler saveDataHandler)
        {
            _saveLoadService = saveLoadService;
            _saveDataHandler = saveDataHandler;
        }

        public void Init()
        {
            Debug.Log("Init authorization service");
            GP_Player.OnLoginComplete += OnLoginComplete;
            GP_Player.OnLoginError += OnLoginError;
        }

        public bool IsAuthorized()
        {
            return GP_Player.IsLoggedIn();
        }

        public void Login(Action onLoginComplete, Action onLoginError)
        {
            _onLoginComplete = onLoginComplete;
            _onLoginError = onLoginError;
            GP_Player.Login();
        }

        private void OnLoginComplete()
        {
            Debug.Log("logged in!");

            PlayerPrefs.DeleteKey("save_data");
            _saveLoadService.LoadData((saveData) =>
            {
                if ( saveData == null )
                {
                    return;
                }

                _saveDataHandler.SaveData = saveData;
                foreach (var saveReader in _saveDataHandler.SaveReaders)
                {
                    saveReader.ReadSave(_saveDataHandler.SaveData);
                }
            });

            _onLoginComplete?.Invoke();
        }

        private void OnLoginError()
        {
            Debug.Log("login error");
            _onLoginError?.Invoke();
        }
    }
}
#endif