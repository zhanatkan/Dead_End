using Cysharp.Threading.Tasks;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Managers.GameManager;
using Game.Scripts.Game.Managers.MiddleManager;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Maps;
using UnityEngine;

namespace Game.Scripts.Base.Services.GameFactory
{
    public interface IGameFactory
    {
        GameField CreateGameField();
        MiddleGameField CreateMiddleGameField();
        PlayerController CreatePlayer();
        FirstPersonCamera CreateCamera(Transform parent);
        Transform CreateWorldCanvas();
        UniTask<Map> CreateMap(string mapName);
    }
}