using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.GameplayControllers;
using Game.Scripts.Game.GameplayControllers.Inventory;
using VContainer;

namespace Game.Scripts.UIScripts.Game
{
    public class GameUIManager
    {
        private readonly InventoryController _inventoryController;
        private readonly PlayerController _playerController;
        private readonly PauseController _pauseController;
        private readonly GameUI _gameUI;
        
        [Inject]
        public GameUIManager(GameUI gameUI, InventoryController inventoryController,
            PlayerController playerController, PauseController pauseController)
        {
            _gameUI = gameUI;
            _inventoryController = inventoryController;
            _playerController = playerController;
            _pauseController = pauseController;
        }
        
        public void Init()
        {
            _gameUI.Init();
            _playerController.OnInventoryWindowOpen += OpenInventoryWindow;
            _gameUI.OnPauseButtonClick += OpenPauseWindow;
        }

        public void DeInit()
        {
            _gameUI.DeInit();
            _playerController.OnInventoryWindowOpen -= OpenInventoryWindow;
            _gameUI.OnPauseButtonClick -= OpenPauseWindow;
        }

        public void SetActive(bool isActive)
        {
            _gameUI.gameObject.SetActive(isActive);
        }
        
        private void OpenPauseWindow()
        {
            _pauseController.OpenPauseWindow();
        }
        
        private void OpenInventoryWindow()
        {
            _gameUI.SetQuickWidgetOverrideCanvas(true);
            _inventoryController.OpenInventoryWindow(() =>
            {
                _gameUI.SetQuickWidgetOverrideCanvas(false);
            });
        }
    }
}