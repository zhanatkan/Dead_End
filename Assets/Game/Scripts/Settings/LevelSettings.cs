using System.Collections.Generic;
using Game.Scripts.UIScripts.Windows.LevelChoice;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(LevelSettings),  menuName = "Settings/" + nameof(LevelSettings))]
    public class LevelSettings : ScriptableObject
    {
        public List<LevelName> LevelNames;
    }
}