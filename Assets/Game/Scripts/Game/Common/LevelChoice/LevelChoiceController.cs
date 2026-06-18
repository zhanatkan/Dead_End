using System;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SpriteSetup;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.UIScripts.Windows.LevelChoice;
using Game.Scripts.Data;
using VContainer;

namespace Game.Scripts.Game.Common.LevelChoice
{
    public class LevelChoiceController : ISaveWriter
    {
        private readonly IWindowManager _windowManager;
        private readonly ISpriteSetupProvider _spriteSetupProvider;
        private readonly IAudioService _audioService;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveLoadService _saveLoadService;
        
        private LevelChoiceWindow _levelChoiceWindow;
        private LevelName _levelName;
        private Action _onLevelChooseEvent;
        
        [Inject]
        public LevelChoiceController(IWindowManager windowManager, IAudioService audioService,
            ISpriteSetupProvider spriteSetupProvider, ISettingsProvider settingsProvider,
            ISaveLoadService saveLoadService)
        {
            _windowManager = windowManager;
            _audioService = audioService;
            _spriteSetupProvider = spriteSetupProvider;
            _settingsProvider = settingsProvider;
            _saveLoadService = saveLoadService;
        }

        public void Init(Action onLevelChoose)
        {
            _onLevelChooseEvent = onLevelChoose;
            _levelChoiceWindow = _windowManager.CreateWindow<LevelChoiceWindow>();
            _levelChoiceWindow.Init(_audioService, _settingsProvider, _spriteSetupProvider, OnLevelChoose);
        }

        public void WriteSave(SaveData saveData)
        {
            saveData.PlayerSaveData.LevelName = _levelName;
        }

        public void ShowLevelChoiceWindow()
        {
            _levelChoiceWindow.Show();
        }

        private void OnLevelChoose()
        {
            _levelName = _levelChoiceWindow.GetLevelsListView.CurrentLevel;
            _saveLoadService.SaveData(null);
            _onLevelChooseEvent?.Invoke();
        }
    }
}