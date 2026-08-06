using System;

namespace Game.Scripts.Base.Services.Authorization
{
    public interface IAuthorizationService
    {
        void Init();
        bool IsAuthorized();
        void Login(Action onLoginComplete, Action onLoginError);
    }
}