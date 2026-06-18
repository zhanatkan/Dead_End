using Cysharp.Threading.Tasks;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Managers.GameField;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.Game.GameField;
using Game.Scripts.Settings.Inventory;
using UnityEngine;

namespace Game.Scripts.Base.Services.GameFactory
{
    public interface IGameFactory
    {
        MainGameField CreateGameField();
        MiddleGameField CreateMiddleGameField();
        PlayerController CreatePlayer();
        FirstPersonCamera CreateCamera(Transform parent);
        Transform CreateWorldCanvas();
        UniTask<Map> CreateMap(string mapName);
        
        ItemPickup CreateItemPickup(ItemType itemType);
    }
}