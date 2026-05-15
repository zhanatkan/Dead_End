using UnityEngine;

namespace Game.Scripts.UIScripts.Windows
{
    public class WindowBackground : MonoBehaviour
    {
        public void Activate(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}