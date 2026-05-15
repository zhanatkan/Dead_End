using UnityEngine;

namespace Game.Scripts.Base
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject MenuLoadingScreen;
        
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show(bool isGame)
        {
            gameObject.SetActive(true);
            MenuLoadingScreen.SetActive(isGame);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}