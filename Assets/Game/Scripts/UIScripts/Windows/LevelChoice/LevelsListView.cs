using System;
using System.Collections.Generic;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SpriteSetup;
using Game.Scripts.Settings;
using Game.Scripts.Settings.Sprites;
using UnityEngine;

namespace Game.Scripts.UIScripts.Windows.LevelChoice
{
    public class LevelsListView : MonoBehaviour
    {
        public event Action OnLevelChoose;

        [SerializeField] private Transform LevelViewParent;
        [SerializeField] private LevelView LevelViewPrefab;
        private ISpriteSetupProvider _spriteSetupProvider;
        private IAudioService _audioService;

        private LevelName _currentLevel;
        private bool _finalLevelChosen;
        private bool _anyLevelChosen;

        private readonly List<LevelView> _levelViews = new();
        public LevelName CurrentLevel => _currentLevel;
        public bool AnyLevelChosen => _anyLevelChosen;
        
        public void CreateLevelViews(ISettingsProvider settingsProvider, ISpriteSetupProvider spriteSetupProvider,
            IAudioService audioService)
        {
            _spriteSetupProvider = spriteSetupProvider;
            _audioService = audioService;

            var levelsSetting = settingsProvider.LevelSettings;
            _finalLevelChosen = false;
            _anyLevelChosen = false;
            
            CreateAllLevelViews(levelsSetting);
        }

        public void Init()
        {
            foreach (var levelView in _levelViews)
            {
                levelView.Init(_audioService);
                levelView.OnLevelChoiceButtonClick += ChooseLevel;
            }
        }

        public void DeInit()
        {
            foreach (var levelView in _levelViews)
            {
                levelView.OnLevelChoiceButtonClick -= ChooseLevel;
                levelView.DeInit();
            }
        }

        public void ResetLevels()
        {
            foreach (var levelView in _levelViews)
            {
                levelView.SetLevelChoice(false);
                levelView.UpdateView();
                _finalLevelChosen = false;
                _anyLevelChosen = false;
            }
        }

        public void SetFinalLevelChosen(bool finalLevelChosen)
        {
            _finalLevelChosen = finalLevelChosen;
        }

        private void CreateAllLevelViews(LevelSettings levelSettings)
        {
            foreach (var levelCard in levelSettings.LevelNames)
            {
                var levelView = Instantiate(LevelViewPrefab, LevelViewParent);
                var levelsSpriteSetup = _spriteSetupProvider.GetSpriteSetup<LevelsSpriteSetup>();
                var levelIcon = levelsSpriteSetup.GetLevelSpriteSetupByType(levelCard).LevelIcon;
                
                levelView.Setup(levelCard, levelIcon);
                _levelViews.Add(levelView);
            }
        }
        
        private void UpdateAllLevelViews(LevelName levelName)
        {
            foreach (var levelView in _levelViews)
            {
                if (levelView.LevelName != levelName && levelView.IsChosen)
                {
                    levelView.SetLevelChoice(false);
                }
                else if (levelView.LevelName == levelName && !levelView.IsChosen)
                {
                    levelView.SetLevelChoice(true);
                }
                levelView.UpdateView();
            }
        }

        private void ChooseLevel(LevelName levelName)
        {
            _anyLevelChosen = true;
            UpdateAllLevelViews(levelName);
            _currentLevel = levelName;
            OnLevelChoiceButtonClick();
        }

        private void OnLevelChoiceButtonClick()
        {
            OnLevelChoose?.Invoke();
        }
    }
}