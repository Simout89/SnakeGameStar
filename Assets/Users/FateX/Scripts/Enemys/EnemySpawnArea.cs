using UnityEngine;

namespace Users.FateX.Scripts.Entity
{
    public class EnemySpawnArea : MonoBehaviour
    {
        [SerializeField] private Transform centerPoint;

        [SerializeField] private float sizeX;
        [SerializeField] private float sizeY;

        [SerializeField] private float checkRadius = 0.5f;

        private Vector3 lastSpawnPos;

        public Vector3 GetRandomPositionOnBorder()
        {
            Vector3 pos = GenerateRandomBorderPosition();

            Collider2D hit = Physics2D.OverlapCircle(pos, checkRadius, LayerMask.GetMask("Wall"));

            if (hit == null)
            {
                lastSpawnPos = pos;
                return pos;
            }

            return lastSpawnPos;
        }

        private Vector3 GenerateRandomBorderPosition()
        {
            float halfX = sizeX / 2f;
            float halfY = sizeY / 2f;
            Vector3 pos = centerPoint.position;

            int side = Random.Range(0, 4);
            switch (side)
            {
                case 0:
                    pos += new Vector3(Random.Range(-halfX, halfX), halfY, 0);
                    break;
                case 1:
                    pos += new Vector3(Random.Range(-halfX, halfX), -halfY, 0);
                    break;
                case 2:
                    pos += new Vector3(-halfX, Random.Range(-halfY, halfY), 0);
                    break;
                case 3:
                    pos += new Vector3(halfX, Random.Range(-halfY, halfY), 0);
                    break;
            }

            return pos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(centerPoint.position - Vector3.down * sizeY / 2 + Vector3.right * sizeX / 2,
                centerPoint.position - Vector3.down * sizeY / 2 - Vector3.right * sizeX / 2);
            Gizmos.DrawLine(centerPoint.position + Vector3.down * sizeY / 2 + Vector3.right * sizeX / 2,
                centerPoint.position + Vector3.down * sizeY / 2 - Vector3.right * sizeX / 2);
            Gizmos.DrawLine(centerPoint.position + Vector3.right * sizeX / 2 + Vector3.up * sizeY / 2,
                centerPoint.position + Vector3.right * sizeX / 2 - Vector3.up * sizeY / 2);
            Gizmos.DrawLine(centerPoint.position - Vector3.right * sizeX / 2 + Vector3.up * sizeY / 2,
                centerPoint.position - Vector3.right * sizeX / 2 - Vector3.up * sizeY / 2);
        }
    }
}