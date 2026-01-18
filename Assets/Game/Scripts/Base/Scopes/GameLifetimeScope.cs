using System.ComponentModel;
using Game.Scripts.Base.Services.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SettingsProvider>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}