using System;
using Game.Scripts.Data;

namespace Game.Scripts.Base.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void Init();
        void DeInit();
        void SaveData(Action onComplete);
        void LoadData(Action<SaveData> onComplete);
        void ResetSave();
    }
}