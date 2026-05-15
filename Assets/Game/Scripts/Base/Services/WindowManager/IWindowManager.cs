using Game.Scripts.UIScripts.Windows;

namespace Game.Scripts.Base.Services.WindowManager
{
    public interface IWindowManager
    {
        void Init();
        T GetTopWindow<T>() where T : BaseWindow;
        T CreateWindow<T>(bool withBackground = true) where T : BaseWindow;
        void HideAllWindows();
    }
}