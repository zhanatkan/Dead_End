using Game.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Game.Scripts.Base.Services.WindowHolder
{
    public interface IWindowHolder
    {
        WindowBackground GetWindowBackground();
        void CreateWindowsRoot();
        T GetWindow<T>() where T : BaseWindow;
    }
}