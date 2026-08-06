using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "AdvertisementSetting", menuName = "Settings/AdvertisementSetting", order = 4)]
    public class AdvertisementSetting : ScriptableObject
    {
        public float InterstitialInterval = 300f;
    }
}