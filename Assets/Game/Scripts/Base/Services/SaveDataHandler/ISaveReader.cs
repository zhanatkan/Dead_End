using Game.Scripts.Data;

namespace Game.Scripts.Base.Services.SaveDataHandler
{
    public interface ISaveReader
    {
        void ReadSave(SaveData saveData);
    }
}