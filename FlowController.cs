using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour
{
    GridManager gridManager;
    GameManager gameManager;
    Camera      mainCamera;

    int              activeColor = -1;
    List<Vector2Int> activePath  = new List<Vector2Int>();

    Dictionary<int, List<Vector2Int>> completedFlows = new Dictionary<int, List<Vector2Int>>();
    Dictionary<int, List<Vector2Int>> partialFlows   = new Dictionary<int, List<Vector2Int>>();

    public int CompletedFlowCount => completedFlows.Count;

    public void Init(GridManager gm, GameManager gam)
    {
        gridManager = gm;
        gameManager = gam;
        mainCamera  = Camera.main;
    }

    public void ResetFlows()
    {
        for (int y = 0; y < gridManager.GridSize; y++)
        for (int x = 0; x < gridManager.GridSize; x++)
            gridManager.GetCell(new Vector2Int(x, y))?.ClearPipe();

        activeColor = -1;
        activePath.Clear();
        completedFlows.Clear();
        partialFlows.Clear();
    }

    void Update()
    {
        if (gameManager.IsLevelComplete) return;

        if      (Input.GetMouseButtonDown(0)) HandlePress();
        else if (Input.GetMouseButton(0))     HandleDrag();
        else if (Input.GetMouseButtonUp(0))   HandleRelease();
    }

    void HandlePress()
    {
        var pos  = MouseToGrid();
        if (!gridManager.IsValid(pos)) return;

        Cell cell = gridManager.GetCell(pos);
        if (cell.dotColor < 0) return;

        ClearActiveVisuals();
        ClearFlowVisuals(cell.dotColor);

        activeColor = cell.dotColor;
        activePath.Add(pos);
        RefreshPathVisuals(activePath, activeColor);
    }

    void HandleDrag()
    {
        if (activeColor < 0 || activePath.Count == 0) return;

        var pos  = MouseToGrid();
        var last = activePath[activePath.Count - 1];

        if (pos == last || !gridManager.IsValid(pos) || !IsAdjacent(last, pos)) return;

        int existingIdx = activePath.IndexOf(pos);
        if (existingIdx >= 0)
        {
            for (int i = activePath.Count - 1; i > existingIdx; i--)
            {
                gridManager.GetCell(activePath[i])?.ClearPipe();
                activePath.RemoveAt(i);
            }
            RefreshPathVisuals(activePath, activeColor);
            return;
        }

        Cell target = gridManager.GetCell(pos);

        if (target.pipeColor >= 0 && target.pipeColor != activeColor)
            ClearFlowVisuals(target.pipeColor);

        if (target.dotColor >= 0 && target.dotColor != activeColor) return;

        activePath.Add(pos);
        RefreshPathVisuals(activePath, activeColor);

        if (target.dotColor == activeColor && activePath.Count > 1)
        {
            var completedPath  = new List<Vector2Int>(activePath);
            int completedColor = activeColor;

            gridManager.GetCell(completedPath[0])?.SetCompleted(true);
            target.SetCompleted(true);

            completedFlows[completedColor] = completedPath;
            partialFlows.Remove(completedColor);

            activeColor = -1;
            activePath.Clear();

            gameManager.OnFlowCompleted();
        }
    }

    void HandleRelease()
    {
        if (activeColor < 0) return;

        if (activePath.Count > 1)
            partialFlows[activeColor] = new List<Vector2Int>(activePath);

        activeColor = -1;
        activePath.Clear();
    }

    void RefreshPathVisuals(List<Vector2Int> path, int colorIdx)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int pos = path[i];
            bool up    = HasNeighbour(path, i, Vector2Int.up);
            bool down  = HasNeighbour(path, i, Vector2Int.down);
            bool left  = HasNeighbour(path, i, Vector2Int.left);
            bool right = HasNeighbour(path, i, Vector2Int.right);
            gridManager.GetCell(pos)?.SetPipeDirectional(colorIdx, up, down, left, right);
        }
    }

    static bool HasNeighbour(List<Vector2Int> path, int idx, Vector2Int dir)
    {
        Vector2Int n = path[idx] + dir;
        if (idx > 0              && path[idx - 1] == n) return true;
        if (idx < path.Count - 1 && path[idx + 1] == n) return true;
        return false;
    }

    void ClearActiveVisuals()
    {
        foreach (var p in activePath)
            gridManager.GetCell(p)?.ClearPipe();
        activePath.Clear();
        activeColor = -1;
    }

    void ClearFlowVisuals(int colorIdx)
    {
        if (completedFlows.TryGetValue(colorIdx, out var path))
        {
            foreach (var p in path)
            {
                gridManager.GetCell(p)?.ClearPipe();
                gridManager.GetCell(p)?.SetCompleted(false);
            }
            completedFlows.Remove(colorIdx);
        }
        if (partialFlows.TryGetValue(colorIdx, out path))
        {
            foreach (var p in path) gridManager.GetCell(p)?.ClearPipe();
            partialFlows.Remove(colorIdx);
        }
    }

    Vector2Int MouseToGrid()
    {
        Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return gridManager.WorldToGrid(world);
    }

    static bool IsAdjacent(Vector2Int a, Vector2Int b)
        => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
}