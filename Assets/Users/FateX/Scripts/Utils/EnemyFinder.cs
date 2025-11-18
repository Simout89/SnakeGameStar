using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
    public static class EnemyFinder
    {
        private static Collider2D[] colliderBuffer = new Collider2D[32];

        public static EnemyBase GetRandomEnemy(Vector3 position, float radius)
        {
            int validCount = GetValidEnemies(position, radius, out int totalCount);

            if (validCount == 0) return null;

            int targetIndex = Random.Range(0, validCount);
            return FindNthValidEnemy(targetIndex, totalCount);
        }

        public static EnemyBase GetNearestEnemy(Vector3 position, float radius)
        {
            int validCount = GetValidEnemies(position, radius, out int totalCount);

            if (validCount == 0) return null;

            EnemyBase nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < totalCount; i++)
            {
                if (colliderBuffer[i].TryGetComponent(out EnemyBase enemy) && IsValidEnemy(enemy))
                {
                    float distance = Vector3.Distance(enemy.transform.position, position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }

            return nearestEnemy;
        }

        private static int GetValidEnemies(Vector3 position, float radius, out int totalCount)
        {
            totalCount = Physics2D.OverlapCircleNonAlloc(position, radius, colliderBuffer);

            if (totalCount == 0) return 0;

            int validCount = 0;

            for (int i = 0; i < totalCount; i++)
            {
                if (colliderBuffer[i].TryGetComponent(out EnemyBase enemy) && IsValidEnemy(enemy))
                {
                    validCount++;
                }
            }

            return validCount;
        }

        private static EnemyBase FindNthValidEnemy(int targetIndex, int totalCount)
        {
            int currentValid = 0;

            for (int i = 0; i < totalCount; i++)
            {
                if (colliderBuffer[i].TryGetComponent(out EnemyBase enemy) && IsValidEnemy(enemy))
                {
                    if (currentValid == targetIndex)
                    {
                        return enemy;
                    }

                    currentValid++;
                }
            }

            return null;
        }
        
        public static EnemyBase[] GetNearestEnemies(Vector3 position, float radius, int count)
        {
            int validCount = GetValidEnemies(position, radius, out int totalCount);

            if (validCount == 0) return System.Array.Empty<EnemyBase>();

            int resultCount = Mathf.Min(count, validCount);
            EnemyBase[] result = new EnemyBase[resultCount];
            float[] distances = new float[resultCount];

            for (int i = 0; i < resultCount; i++)
            {
                distances[i] = float.MaxValue;
            }

            for (int i = 0; i < totalCount; i++)
            {
                if (colliderBuffer[i].TryGetComponent(out EnemyBase enemy) && IsValidEnemy(enemy))
                {
                    float distance = Vector3.Distance(enemy.transform.position, position);

                    for (int j = 0; j < resultCount; j++)
                    {
                        if (distance < distances[j])
                        {
                            for (int k = resultCount - 1; k > j; k--)
                            {
                                distances[k] = distances[k - 1];
                                result[k] = result[k - 1];
                            }

                            distances[j] = distance;
                            result[j] = enemy;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static EnemyBase[] GetRandomEnemies(Vector3 position, float radius, int count)
        {
            int validCount = GetValidEnemies(position, radius, out int totalCount);

            if (validCount == 0) return System.Array.Empty<EnemyBase>();

            int resultCount = Mathf.Min(count, validCount);
            EnemyBase[] result = new EnemyBase[resultCount];
            bool[] used = new bool[validCount];

            for (int i = 0; i < resultCount; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, validCount);
                } while (used[randomIndex]);

                used[randomIndex] = true;
                result[i] = FindNthValidEnemy(randomIndex, totalCount);
            }

            return result;
        }

        private static bool IsValidEnemy(EnemyBase enemy)
        {
            return enemy.Visible && !enemy.AlreadyDie;
        }
    }
}