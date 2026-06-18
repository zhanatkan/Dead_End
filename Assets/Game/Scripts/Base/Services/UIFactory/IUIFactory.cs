using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.UIScripts.Game;
using Game.Scripts.UIScripts.MiddleGame; 
using Game.Scripts.UIScripts.MainMenu;
using Game.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Game.Scripts.Base.Services.UIFactory
{
    public interface IUIFactory
    {
        void CreateWindowsRoot();
        List<BaseWindow> SetupWindows(List<GameObject> windowObjects);
        WindowBackground CreateWindowBackground();

        Transform CreateUICanvasRoot();
        MainMenuUI CreateMainMenuUI(Transform uiCanvas);
        MiddleGameUI CreateMiddleGameUI(Transform parent);
        GameUI CreateGameUI(Transform parent);
    }
}