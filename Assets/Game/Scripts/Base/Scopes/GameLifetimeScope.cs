using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SettingsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveDataHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveLoadService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<StatesFactory>(Lifetime.Singleton).WithParameter<LifetimeScope>(this);
            builder.Register<StateMachine>(Lifetime.Singleton);
        }
    }
}