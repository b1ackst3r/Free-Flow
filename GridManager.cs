using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int GridSize { get; private set; }

    Cell[,] cells;

    public void BuildGrid(int size)
    {
        GridSize = size;
        cells    = new Cell[size, size];

        Cell.EnsureSprites();

        float offset = (size - 1) / 2f;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            var go = new GameObject();
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(x - offset, y - offset, 0f);

            var cell = go.AddComponent<Cell>();
            cell.Init(new Vector2Int(x, y));
            cells[x, y] = cell;
        }
    }

    public void ClearGrid()
    {
        if (cells == null) return;
        for (int y = 0; y < GridSize; y++)
        for (int x = 0; x < GridSize; x++)
            if (cells[x, y] != null)
                Destroy(cells[x, y].gameObject);
        cells = null;
    }

    public Cell GetCell(Vector2Int pos)
    {
        if (!IsValid(pos)) return null;
        return cells[pos.x, pos.y];
    }

    public bool IsValid(Vector2Int pos)
        => pos.x >= 0 && pos.x < GridSize && pos.y >= 0 && pos.y < GridSize;

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        float offset = (GridSize - 1) / 2f;
        int x = Mathf.RoundToInt(worldPos.x + offset);
        int y = Mathf.RoundToInt(worldPos.y + offset);
        return new Vector2Int(x, y);
    }
}