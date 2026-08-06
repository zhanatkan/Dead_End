using System;
using UnityEngine;
using Game.Scripts.Game.Character.Player;

namespace Game.Scripts.Game.GameField
{
    public class TriggerPlace : MonoBehaviour
    {
        [SerializeField] private TriggerHandler TriggerEventsHandler;
        private Action _onTriggerPlaceEnter;
        
        public void Init(Action onEnter)
        {
            _onTriggerPlaceEnter = onEnter;
            
            TriggerEventsHandler.OnTriggerEnterEvent += OnEnter;
            TriggerEventsHandler.OnTriggerStayEvent += OnEnter;
        }
                
        public void DeInit()
        {
            TriggerEventsHandler.OnTriggerEnterEvent += OnEnter;
            TriggerEventsHandler.OnTriggerStayEvent -= OnEnter;
        }
                
        private void OnEnter(Collider obj)
        {
            if (obj.gameObject.GetComponent<PlayerController>())
            {
                _onTriggerPlaceEnter?.Invoke();
            }
        }
    }
}