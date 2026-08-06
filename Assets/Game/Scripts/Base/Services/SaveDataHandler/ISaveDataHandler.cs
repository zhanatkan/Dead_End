using System.Collections.Generic;
using Game.Scripts.Data;

namespace Game.Scripts.Base.Services.SaveDataHandler
{
    public interface ISaveDataHandler
    {
        SaveData SaveData { get; set; }
        HashSet<ISaveWriter> SaveWriters { get; }
        HashSet<ISaveReader> SaveReaders { get; }

        void RegisterSaveWriter(ISaveWriter saveWriter);
        void RegisterSaveReader(ISaveReader saveReader);
        void CleanUp();
    }
}