using System;
using UnityEngine;

namespace Game.Scripts.Game.GameField
{
    public class TriggerHandler : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterEvent, OnTriggerStayEvent, OnTriggerExitEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(other);
        }
    }
}