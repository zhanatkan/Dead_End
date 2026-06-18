using UnityEngine;

namespace Game.Scripts.Game.GameField
{
    public class SpawnRange : MonoBehaviour, ISpawnPosition, IRandomPosition
    {
        [SerializeField] private Transform minPoint, maxPoint;
        
        private Vector3 SpawnRangeSize => maxPoint.position - minPoint.position;
        private Vector3 Center => minPoint.position + (maxPoint.position - minPoint.position) / 2;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Center, SpawnRangeSize);
        }

        public Vector3 SpawnPosition()
        {
            var x = Random.Range(minPoint.position.x, maxPoint.position.x);
            var y = Random.Range(minPoint.position.y, maxPoint.position.y);
            var z = Random.Range(minPoint.position.z, maxPoint.position.z);
            return new Vector3(x, y, z);
        }

        public Vector3 RandomPosition()
        {
            return SpawnPosition();
        }
    }
}