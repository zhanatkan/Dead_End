using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Common.LevelChoice;
using Game.Scripts.Game.GameplayControllers;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.Game.Managers.GameField;
using VContainer;

namespace Game.Scripts.UIScripts.MiddleGame
{
    public class MiddleGameUIManager
    {
        private readonly LevelChoiceController _levelChoiceController;
        private readonly InventoryController _inventoryController;
        private readonly PlayerController _playerController;
        private readonly MiddleGameUI _middleGameUI;
        private readonly MiddleGameField _middleGameField;
        private readonly PauseController _pauseController;
        
        [Inject]
        public MiddleGameUIManager(MiddleGameUI middleGameUI, InventoryController inventoryController,
            PlayerController playerController, LevelChoiceController levelChoiceController,
            MiddleGameField middleGameField, PauseController pauseController)
        {
            _middleGameUI = middleGameUI;
            _inventoryController = inventoryController;
            _playerController = playerController;
            _levelChoiceController = levelChoiceController;
            _middleGameField = middleGameField;
            _pauseController = pauseController;
        }
        
        public void Init()
        {
            _middleGameUI.Init();
            _playerController.OnInventoryWindowOpen += OpenInventoryWindow;
            _middleGameField.OnLevelChoiceTriggerEnterEvent += OpenLevelChoiceWindow;
            _middleGameUI.OnPauseButtonClick += OpenPauseWindow;
        }

        public void DeInit()
        {
            _middleGameUI.DeInit();
            _playerController.OnInventoryWindowOpen -= OpenInventoryWindow;
            _middleGameField.OnLevelChoiceTriggerEnterEvent -= OpenLevelChoiceWindow;
            _middleGameUI.OnPauseButtonClick += OpenPauseWindow;
        }

        public void SetActive(bool isActive)
        {
            _middleGameUI.gameObject.SetActive(isActive);
        }

        private void OpenInventoryWindow()
        {
            _middleGameUI.SetQuickWidgetOverrideCanvas(true);
            _inventoryController.OpenInventoryWindow(() =>
            {
                _middleGameUI.SetQuickWidgetOverrideCanvas(false);
            });
        }
        
        private void OpenLevelChoiceWindow()
        {
            _levelChoiceController.ShowLevelChoiceWindow();
        }
        
        private void OpenPauseWindow()
        {
            _pauseController.OpenPauseWindow();
        }
    }
}