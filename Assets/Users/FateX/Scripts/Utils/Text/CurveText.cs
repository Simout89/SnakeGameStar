using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class CurveText : MonoBehaviour
{
    [Header("Настройки изгиба")]
    [SerializeField] private float curveStrength = 5f;
    [SerializeField] private float fullLineWidth = 500f; // Ширина полной строки
    [SerializeField] private AnimationCurve curvature = AnimationCurve.Linear(0, 0, 1, 1);
    
    private TMP_Text textMesh;
    private bool hasTextChanged;

    void OnEnable()
    {
        textMesh = GetComponent<TMP_Text>();
        if (textMesh != null)
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
            hasTextChanged = true;
        }
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void OnTextChanged(Object obj)
    {
        if (obj == textMesh)
        {
            hasTextChanged = true;
        }
    }

    void LateUpdate()
    {
        if (textMesh != null && hasTextChanged)
        {
            hasTextChanged = false;
            ApplyCurve();
        }
    }

    void ApplyCurve()
    {
        if (textMesh == null)
            return;

        textMesh.ForceMeshUpdate();
        
        TMP_TextInfo textInfo = textMesh.textInfo;
        
        if (textInfo == null || textInfo.characterCount == 0)
            return;

        // Находим границы текста по X
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;
                
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int materialIndex = charInfo.materialReferenceIndex;
            
            if (materialIndex >= textInfo.meshInfo.Length)
                continue;
                
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            
            if (vertices == null || charInfo.vertexIndex + 3 >= vertices.Length)
                continue;
            
            for (int j = 0; j < 4; j++)
            {
                float x = vertices[charInfo.vertexIndex + j].x;
                minX = Mathf.Min(minX, x);
                maxX = Mathf.Max(maxX, x);
            }
        }
        
        float textWidth = maxX - minX;
        
        if (textWidth <= 0)
            return;
        
        // Центр текста
        float centerX = (minX + maxX) / 2f;
        
        // Применяем изгиб к каждому символу
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;
            
            if (materialIndex >= textInfo.meshInfo.Length)
                continue;
            
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            
            if (vertices == null || vertexIndex + 3 >= vertices.Length)
                continue;
            
            // Обрабатываем каждую вершину символа
            for (int j = 0; j < 4; j++)
            {
                Vector3 originalVertex = vertices[vertexIndex + j];
                
                // Расстояние от центра текста
                float offsetFromCenter = originalVertex.x - centerX;
                
                // Нормализуем относительно ПОЛНОЙ ширины строки, а не текущего текста
                float normalizedX = 0.5f + (offsetFromCenter / fullLineWidth);
                
                // Ограничиваем значение от 0 до 1
                normalizedX = Mathf.Clamp01(normalizedX);
                
                // Вычисляем смещение по Y
                float curveValue = curvature.Evaluate(normalizedX);
                float yOffset = curveValue * curveStrength;
                
                vertices[vertexIndex + j] = new Vector3(
                    originalVertex.x,
                    originalVertex.y + yOffset,
                    originalVertex.z
                );
            }
        }
        
        // Обновляем геометрию с проверками
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            if (textInfo.meshInfo[i].mesh != null && textInfo.meshInfo[i].vertices != null)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }
    }

    void OnValidate()
    {
        if (textMesh != null)
        {
            hasTextChanged = true;
        }
    }
}