using System;

namespace Game.Scripts.Base.Services.Authorization
{
    public class MockAuthorizationService : IAuthorizationService
    {
        public void Init()
        {
        }

        public bool IsAuthorized()
        {
            return false;
        }

        public void Login(Action onLoginComplete, Action onLoginError)
        {
            onLoginComplete?.Invoke();
        }
    }
}