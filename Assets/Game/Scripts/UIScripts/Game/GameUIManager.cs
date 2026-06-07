using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.GameplayControllers.Inventory;
using UnityEngine;
using VContainer;

namespace Game.Scripts.UIScripts.Game
{
    public class GameUIManager
    {
        private readonly InventoryController _inventoryController;
        private readonly PlayerController _playerController;
        private readonly GameUI _gameUI;
        
        [Inject]
        public GameUIManager(GameUI gameUI, InventoryController inventoryController,
            PlayerController playerController)
        {
            _gameUI = gameUI;
            _inventoryController = inventoryController;
            _playerController = playerController;
        }
        
        public void Init()
        {
            _gameUI.Init();
            //SetActive(true);
            _playerController.OnInventoryWindowOpen += OpenInventoryWindow;
        }

        public void DeInit()
        {
            _gameUI.DeInit();
            _playerController.OnInventoryWindowOpen -= OpenInventoryWindow;
        }

        public void SetActive(bool isActive)
        {
            _gameUI.gameObject.SetActive(isActive);
        }

        private void OpenInventoryWindow()
        {
            _inventoryController.OpenInventoryWindow();
        }
    }
}