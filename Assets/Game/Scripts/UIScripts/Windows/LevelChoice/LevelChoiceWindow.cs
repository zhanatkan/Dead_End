using System;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SpriteSetup;
using UnityEngine;

namespace Game.Scripts.UIScripts.Windows.LevelChoice
{
    public class LevelChoiceWindow : BaseWindow
    {
        [SerializeField] private ButtonWithClickSound CloseButton;
        [SerializeField] private ButtonWithClickSound ChooseLevelButton;
        [SerializeField] private LevelsListView LevelsListView;
        
        private IAudioService _audioService;
        private ISettingsProvider _settingsProvider;
        private ISpriteSetupProvider _spriteSetupProvider;
        
        private Action _onFinalLevelChoiceButtonClicked;
        
        public LevelsListView GetLevelsListView => LevelsListView;

        public void Init(IAudioService audioService, ISettingsProvider settingsProvider,
            ISpriteSetupProvider spriteSetupProvider, Action onFinalLevelChoiceButtonClicked)
        {
            _audioService = audioService;
            _settingsProvider = settingsProvider;
            _spriteSetupProvider = spriteSetupProvider;
            _onFinalLevelChoiceButtonClicked = onFinalLevelChoiceButtonClicked;
            LevelsListView.CreateLevelViews(_settingsProvider, _spriteSetupProvider, _audioService);
        }

        public override void Show()
        {
            LevelsListView.Init();
            ChooseLevelButton.Init(_audioService, OnFinalLevelChoiceButtonClicked);
            CloseButton.Init(_audioService, OnCloseButtonClicked);
            base.Show();
        }

        public override void Hide()
        {
            LevelsListView.DeInit();
            ChooseLevelButton.DeInit();
            CloseButton.DeInit();
            base.Hide();
        }

        private void OnFinalLevelChoiceButtonClicked()
        {
            if (GetLevelsListView.AnyLevelChosen)
            {
                GetLevelsListView.SetFinalLevelChosen(true);
                Hide();
                _onFinalLevelChoiceButtonClicked?.Invoke();
            }
        }

        private void OnCloseButtonClicked()
        {
            LevelsListView.ResetLevels();
            Hide();
        }
    }
}