using System.Collections.Generic;
using Game.Scripts.Base.Services.WindowHolder;
using Game.Scripts.UIScripts.Windows;
using JetBrains.Annotations;
using VContainer;

namespace Game.Scripts.Base.Services.WindowManager
{
    public sealed class WindowManager : IWindowManager
    {
        private readonly IWindowHolder _windowHolder;
        private readonly Stack<BaseWindow> _windowsStack = new();

        private WindowBackground _windowBackground;

        [Inject]
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
        public T CreateWindow<T>(bool withBackground = true) where T : BaseWindow
        {
            var window = _windowHolder.GetWindow<T>();
            if ( window == null )
            {
                return null;
            }
            
            if ( withBackground )
            {
                _windowBackground.Activate(true);
                _windowBackground.transform.SetSiblingIndex(_windowsStack.Count);
            }
            
            window.OnHideAction += OnHide;
            window.transform.SetSiblingIndex(_windowsStack.Count + 1);
            _windowsStack.Push(window);
            
            return window;
        }

        public void HideAllWindows()
        {
            while ( _windowsStack.Count > 0 )
            {
                _windowsStack.Peek().Hide();
            }
        }

        private void OnHide()
        {
            var hidedWindow = _windowsStack.Pop();
            hidedWindow.OnHideAction -= OnHide;
            
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