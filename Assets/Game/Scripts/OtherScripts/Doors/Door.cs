using UnityEngine;

namespace Game.Scripts.OtherScripts.Doors
{
    public class Door : IOpenable
    {
        public int doorKeyID;
        public Animator doorAnimator;
        public AudioSource doorAudio;

        public void Open()
        {
            doorAnimator.SetBool("IsOpen", true);
            doorAudio.Play();
        }

        public bool CanOpenDoorWithKey(int keyID)
        {
            return keyID == doorKeyID;
        }
    }
}