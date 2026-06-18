using System;
using Game.Scripts.Base.Services.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UIScripts.Windows.LevelChoice
{
    public class LevelView : MonoBehaviour
    {
        public event Action<LevelName> OnLevelChoiceButtonClick;

        [SerializeField] private Image LevelIcon;
        [SerializeField] private ButtonWithClickSound Button;
        [SerializeField] private GameObject DoneCheckObject;
        
        public LevelName LevelName { get; private set; }
        public bool IsChosen { get; private set; }

        public void Setup(LevelName levelName, Sprite levelIcon)
        {
            LevelName = levelName;
            SetLevelIcon(levelIcon);
        }

        public void Init(IAudioService audioService)
        {
            Button.Init(audioService, OnActionButtonClicked);
        }

        public void DeInit()
        {
            Button.DeInit();
        }

        public void UpdateView()
        {
            Button.Interactable = !IsChosen;
            DoneCheckObject.gameObject.SetActive(IsChosen);
        }

        public void SetLevelChoice(bool isChosen)
        {
            IsChosen = isChosen;
        }

        private void SetLevelIcon(Sprite levelIcon)
        {
            LevelIcon.sprite = levelIcon;
        }

        private void OnActionButtonClicked()
        {
            OnLevelChoiceButtonClick?.Invoke(LevelName);
        }
    }
}