using Leopotam.Ecs;
using UnityEngine;

namespace Game.Scripts.Game.AIScripts.Common
{
    public class BotProjectile : MonoBehaviour
    {
        private float _damage;
        private EcsEntity _playerEntity;
        private Transform _playerTransform;
        private LayerMask _botLayerMask;

        public void Init(float damage, EcsEntity playerEntity, Transform playerTransform, LayerMask botLayerMask)
        {
            _damage = damage;
            _playerEntity = playerEntity;
            _playerTransform = playerTransform;
            _botLayerMask = botLayerMask;

            Destroy(gameObject, 3f);
        }

        private void OnTriggerEnter(Collider other)
        {
            int otherLayerBit = 1 << other.gameObject.layer;
            if ((_botLayerMask.value & otherLayerBit) != 0)
            {
                return;
            }

            if (_playerTransform != null && (other.transform == _playerTransform || other.transform.IsChildOf(_playerTransform)))
            {
                if (_playerEntity.IsAlive())
                {
                    ref var damageEvent = ref _playerEntity.Get<TakeDamageEvent>();
                    damageEvent.Damage += _damage;
                }
            }
            Destroy(gameObject);
        }
    }
}