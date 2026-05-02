using System.Collections.Generic;
using VContainer;

namespace Game.Scripts.Base.Services.SaveDataHandler
{
    public class SaveDataHandler : ISaveDataHandler
    {
        public SaveData SaveData { get; set; }
        public HashSet<ISaveWriter> SaveWriters { get; } = new();
        public HashSet<ISaveReader> SaveReaders { get; } = new();

        [Inject]
        public SaveDataHandler()
        { }
        
        public void RegisterSaveWriter(ISaveWriter saveWriter)
        {
            SaveWriters.Add(saveWriter);
        }

        public void RegisterSaveReader(ISaveReader saveReader)
        {
            SaveReaders.Add(saveReader);
        }

        public void CleanUp()
        {
            SaveWriters.Clear();
            SaveReaders.Clear();
        }
    }
}