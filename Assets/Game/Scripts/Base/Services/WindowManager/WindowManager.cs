using System.Collections.Generic;
using Game.Scripts.Base.Services.WindowHolder;
using Game.Scripts.UIScripts.Windows;
using JetBrains.Annotations;

namespace Game.Scripts.Base.Services.WindowManager
{
    public sealed class WindowManager : IWindowManager
    {
        private readonly IWindowHolder _windowHolder;
        private readonly Stack<BaseWindow> _windowsStack = new();

        private WindowBackground _windowBackground;

        public WindowManager(IWindowHolder windowHolder)
        {
            _windowHolder = windowHolder;
        }

        public void Init()
        {
            _windowHolder.CreateWindowsRoot();
            _windowBackground = _windowHolder.GetWindowBackground();
            _windowBackground.Activate(false);
        }

        public void DeInit()
        {
            _windowBackground = null;
        }

        public T GetTopWindow<T>() where T : BaseWindow
        {
            return _windowsStack.Peek() as T;
        }

        [CanBeNull]
        public T CreateWindow<T>() where T : BaseWindow
        {
            var window = _windowHolder.GetWindow<T>();
            if ( window == null )
            {
                return null;
            }
            
            window.OnShowAction += OnShow;
            window.OnHideAction += OnHide;
            
            return window;
        }

        public void HideAllWindows()
        {
            while ( _windowsStack.Count > 0 )
            {
                _windowsStack.Peek().Hide();
            }
        }

        private void OnShow(BaseWindow window)
        {
            _windowBackground.Activate(true);
            _windowBackground.transform.SetSiblingIndex(_windowsStack.Count);
            
            window.transform.SetSiblingIndex(_windowsStack.Count + 1);
            _windowsStack.Push(window);
        }

        private void OnHide()
        {
            _windowsStack.Pop();

            if ( _windowsStack.Count == 0 )
            {
                _windowBackground.Activate(false);
            }
            else
            {
                _windowBackground.transform.SetSiblingIndex(_windowsStack.Count - 1);
            }
        }
    }
}