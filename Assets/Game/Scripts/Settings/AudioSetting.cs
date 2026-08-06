using System;
using System.Collections.Generic;
using Game.Scripts.Base.Services.Audio;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [Serializable]
    public class SoundClipInfo
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
    }

    [CreateAssetMenu(fileName = "AudioSetting", menuName = "Settings/AudioSetting", order = 3)]
    public class AudioSetting : ScriptableObject
    {
        public List<SoundClipInfo> SoundClipInfos;
        public AudioClip MenuMusicClip, GameMusicClip;

        public AudioClip GetSoundClipByName(SoundType soundType)
        {
            foreach (var soundClipInfo in SoundClipInfos)
            {
                if ( soundClipInfo.SoundType == soundType )
                {
                    return soundClipInfo.AudioClip;
                }
            }

            return null;
        }
    }
}