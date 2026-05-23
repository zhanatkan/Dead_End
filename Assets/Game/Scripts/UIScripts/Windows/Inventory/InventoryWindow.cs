using System;
using System.Collections.Generic;
//using Game.Scripts.Settings.Game;
using Game.Scripts.Settings.Sprites;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SpriteSetup;
using Game.Scripts.Data;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UIScripts.Windows.Inventory
{
    public class InventoryWindow : BaseWindow
    {
        [SerializeField] private ButtonWithClickSound QuitButton;
        [SerializeField] private TextMeshProUGUI SlotsInfoText;

        private ISpriteSetupProvider _spriteSetupProvider;
        private IAudioService _audioService;
        private ISettingsProvider _settingProvider;
        private Action _onQuit;

        public void Init(ISpriteSetupProvider spriteSetupProvider,
            IAudioService audioService,
            ISettingsProvider settingProvider,
            Action onQuit)
        {
            _spriteSetupProvider = spriteSetupProvider;
            _audioService = audioService;
            _settingProvider = settingProvider;
            _onQuit = onQuit;
        }

        public override void Show()
        {
            
        }

        public override void Hide()
        {
            QuitButton.DeInit();
            base.Hide();
        }
        
        private void UpdateSlotsInfo()
        {
            //SlotsInfoText.text = $"{_playerSaveData.UsedSlots}/{_configProvider.PlayerConfig.InventoryConfig.MaxInventorySlots}";
        }

        private void OnQuit()
        {
            _onQuit?.Invoke();
            Hide();
        }
    }
}