using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(BotsSetting), menuName = "Settings/" + nameof(BotsSetting))]
    public class BotsSetting : ScriptableObject
    {
        public List<BotSetting> Monsters;
    }
}