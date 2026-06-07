using Game.Scripts.Settings.Inventory;
using UnityEngine;

namespace Game.Scripts.Settings.CharacterSettings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        public CharacterMoveSetting CharacterMoveConfig;
        public KeyCode JumpButton = KeyCode.Space;
        public CameraSettings CameraSettings;
        public InventorySetting InventorySettings;
    }
}