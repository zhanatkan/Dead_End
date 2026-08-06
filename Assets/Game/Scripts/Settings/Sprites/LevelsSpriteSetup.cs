using System;
using System.Collections.Generic;
using Game.Scripts.UIScripts.Windows.LevelChoice;
using UnityEngine;

namespace Game.Scripts.Settings.Sprites
{
    [CreateAssetMenu(fileName = nameof(LevelsSpriteSetup), 
        menuName = "SpriteSetups/" + nameof(LevelsSpriteSetup), order = 3)]
    public class LevelsSpriteSetup : BaseSpriteSetup
    {
        public List<LevelSpriteSetup> LevelSpriteSetups;
        
        public LevelSpriteSetup GetLevelSpriteSetupByType(LevelName levelName)
        {
            foreach (var levelSpriteSetup in LevelSpriteSetups)
            {
                if (levelSpriteSetup.LevelName == levelName)
                {
                    return levelSpriteSetup;
                }
            }
            return null;
        }
    }
    
    [Serializable]
    public class LevelSpriteSetup
    {
        public LevelName LevelName;
        public Sprite LevelIcon;
    }
}