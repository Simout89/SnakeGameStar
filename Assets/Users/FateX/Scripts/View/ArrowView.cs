using UnityEngine;
using System.Collections.Generic;

namespace Users.FateX.Scripts.View
{
    public class ArrowView : MonoBehaviour
    {
        [SerializeField] private float radius = 100; // Радиус круга на UI (в пикселях)
        [SerializeField] private float hideDistance = 0; // Дистанция при которой стрелка пропадает (0 = всегда видна)
        [SerializeField] private RectTransform arrowPrefab; // Префаб UI стрелки
        [SerializeField] private Transform arrowContainer; // Canvas для стрелок
        [SerializeField] private Transform playerTransform; // Позиция игрока/камеры
        [SerializeField] private Camera cam;
        
        private Dictionary<GameObject, RectTransform> arrows = new Dictionary<GameObject, RectTransform>();

        private void Awake()
        {
            if (cam == null) cam = Camera.main;
        }

        public void StartTracking(GameObject obj)
        {
            if (obj == null || arrows.ContainsKey(obj)) return;
            
            RectTransform arrow = Instantiate(arrowPrefab, arrowContainer);
            arrow.gameObject.SetActive(false);
            arrows[obj] = arrow;
        }

        public void StopTracking(GameObject obj)
        {
            if (obj == null || !arrows.ContainsKey(obj)) return;
            
            Destroy(arrows[obj].gameObject);
            arrows.Remove(obj);
        }

        private void Update()
        {
            if (playerTransform == null) return;
            
            foreach (var kvp in arrows)
            {
                if (kvp.Key == null)
                {
                    Destroy(kvp.Value.gameObject);
                    continue;
                }

                float dist = Vector3.Distance(playerTransform.position, kvp.Key.transform.position);
                
                if (hideDistance > 0 && dist <= hideDistance)
                {
                    kvp.Value.gameObject.SetActive(false);
                }
                else
                {
                    ShowArrow(kvp.Value, kvp.Key);
                }
            }
        }

        private void ShowArrow(RectTransform arrow, GameObject target)
        {
            arrow.gameObject.SetActive(true);

            Vector2 dirToTarget = ((Vector2)target.transform.position - (Vector2)playerTransform.position).normalized;
            
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Vector3 arrowPos = screenCenter + new Vector3(dirToTarget.x, dirToTarget.y, 0) * radius;
            
            arrow.position = arrowPos;
            
            float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}