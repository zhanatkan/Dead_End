using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Base.Services.Settings
{
    public sealed class SettingsProvider : ISettingsProvider
    {
        private readonly Dictionary<Type, ScriptableObject> _configs = new();

        private const string SettingsFolder = "Resources/Settings"; 

        public void LoadSettings(Action action)
        {
            _configs.Clear();

            var allConfigs = Resources.LoadAll<ScriptableObject>(SettingsFolder);

            foreach (var config in allConfigs)
            {
                var type = config.GetType();
                _configs.Add(type, config);
            }

            action.Invoke();
        }

        public T Get<T>() where T : ScriptableObject
        {
            if (_configs.TryGetValue(typeof(T), out var config))
            {
                return (T)config;
            }

            throw new Exception($"Config of type {typeof(T)} not loaded.");
        }
    }
}