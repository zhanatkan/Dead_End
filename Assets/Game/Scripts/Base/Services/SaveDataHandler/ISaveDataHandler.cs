using System.Collections.Generic;

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