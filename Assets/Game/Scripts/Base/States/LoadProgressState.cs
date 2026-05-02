using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.States
{
    public sealed class LoadProgressState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ISaveLoadService _saveLoadService;

        [Inject]
        public LoadProgressState(StateMachine stateMachine, ISettingsProvider settingsProvider,
            ISaveDataHandler saveDataHandler, ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _settingsProvider = settingsProvider;
            _saveDataHandler = saveDataHandler;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            _saveLoadService.Init();
            _saveLoadService.LoadData(OnSaveDataLoaded);
        }

        public void Exit()
        {

        }

        private void OnSaveDataLoaded(SaveData saveData)
        {
            if ( saveData == null )
            {
                _saveDataHandler.SaveData = CreateData();
            }
            else
            {
                _saveDataHandler.SaveData = saveData;
            }
            //_stateMachine.Enter<InitServicesState>();
        }


        private SaveData CreateData()
        {
            Debug.Log("Create new save data");

            var saveData = new SaveData();
            return saveData;
        }
    }
}