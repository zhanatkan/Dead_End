using UnityEngine;

namespace Game.Scripts.Base.Services.Settings
{
    public interface ISettingsProvider
    {
        T Get<T>() where T : ScriptableObject;
    }
}