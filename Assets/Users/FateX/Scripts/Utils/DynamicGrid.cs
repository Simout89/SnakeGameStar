using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridWithSpacing : MonoBehaviour
{
    [Header("Сетка")]
    public int columns = 3;            // количество столбцов
    public float aspect = 1f;          // ширина/высота (1 = квадрат)
    
    [Header("Spacing")]
    public float minSpacing = 5f;      // минимальный интервал между ячейками
    public float maxSpacing = 20f;     // максимальный интервал между ячейками

    private GridLayoutGroup grid;
    private RectTransform rt;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        UpdateGrid();
    }

    void UpdateGrid()
    {
        float containerWidth = rt.rect.width - grid.padding.left - grid.padding.right;
        float containerHeight = rt.rect.height - grid.padding.top - grid.padding.bottom;

        int childCount = transform.childCount;
        int rows = Mathf.CeilToInt((float)childCount / columns);

        // --- вычисляем идеальный размер ячейки по ширине ---
        float totalMinSpacingX = (columns - 1) * minSpacing;
        float cellWidth = (containerWidth - totalMinSpacingX) / columns;
        float cellHeight = cellWidth / aspect;

        // --- проверяем ограничение по высоте ---
        float totalMinSpacingY = (rows - 1) * minSpacing;
        float maxCellHeight = (containerHeight - totalMinSpacingY) / rows;
        if (cellHeight > maxCellHeight)
        {
            cellHeight = maxCellHeight;
            cellWidth = cellHeight * aspect;
        }

        // --- динамический spacing ---
        float spacingX = (containerWidth - columns * cellWidth) / Mathf.Max(columns - 1, 1);
        float spacingY = (containerHeight - rows * cellHeight) / Mathf.Max(rows - 1, 1);

        // ограничиваем spacing по min/max
        spacingX = Mathf.Clamp(spacingX, minSpacing, maxSpacing);
        spacingY = Mathf.Clamp(spacingY, minSpacing, maxSpacing);

        grid.cellSize = new Vector2(cellWidth, cellHeight);
        grid.spacing = new Vector2(spacingX, spacingY);
    }
}
