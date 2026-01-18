using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Base.Services.Settings
{
    public sealed class SettingsProvider : ISettingsProvider
    {
        private readonly Dictionary<Type, ScriptableObject> _configs;

        public SettingsProvider(IEnumerable<ScriptableObject> configs)
        {
            _configs = configs.ToDictionary(c => c.GetType());
        }

        public T Get<T>() where T : ScriptableObject
        {
            return (T)_configs[typeof(T)];
        }
    }
}