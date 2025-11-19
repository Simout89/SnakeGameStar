using System.Collections.Generic;
using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
    public static class MyUtils
    {
        public static List<Vector3> GetPositionsInCircle(Vector3 center, float radius, float angleStep)
        {
            List<Vector3> positions = new List<Vector3>();
            int count = Mathf.FloorToInt(360f / angleStep);

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y,
                    center.z + Mathf.Sin(angle) * radius
                );
                positions.Add(pos);
            }

            return positions;
        }
        
        public static List<Vector3> GetPositionsInCircle(Vector3 center, float radius, int pointsCount)
        {
            List<Vector3> positions = new List<Vector3>();
            if (pointsCount <= 0) return positions;

            float angleStep = 360f / pointsCount;

            for (int i = 0; i < pointsCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y,
                    center.z + Mathf.Sin(angle) * radius
                );
                positions.Add(pos);
            }

            return positions;
        }
        
        public static List<Vector3> GetPositionsInCircle2D(Vector3 center, float radius, int pointsCount)
        {
            List<Vector3> positions = new List<Vector3>();
            if (pointsCount <= 0) return positions;

            float angleStep = 360f / pointsCount;

            for (int i = 0; i < pointsCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y + Mathf.Sin(angle) * radius,
                    center.z
                );
                positions.Add(pos);
            }

            return positions;
        }
    }
}