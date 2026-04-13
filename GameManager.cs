using UnityEngine;

public class GameManager : MonoBehaviour
{
    GridManager    gridManager;
    FlowController flowController;
    UIManager      uiManager;

    int currentLevel = 0;
    int totalPairs   = 0;

    public bool IsLevelComplete { get; private set; }

    public void Init(GridManager gm, FlowController fc, UIManager ui)
    {
        gridManager    = gm;
        flowController = fc;
        uiManager      = ui;
    }

    public void LoadLevel(int index)
    {
        IsLevelComplete = false;
        currentLevel    = index;

        var level  = GameData.Levels[index];
        totalPairs = level.pairs.Length;

        gridManager.ClearGrid();
        gridManager.BuildGrid(level.size);
        flowController.Init(gridManager, this);
        flowController.ResetFlows();

        foreach (var pair in level.pairs)
        {
            gridManager.GetCell(pair.a)?.SetDot(pair.colorIdx);
            gridManager.GetCell(pair.b)?.SetDot(pair.colorIdx);
        }

        Camera.main.orthographicSize = level.size / 2f + 1.8f;

        uiManager.ShowLevel(index + 1, GameData.Levels.Length);
        uiManager.HideWin();
    }

    public void OnFlowCompleted()
    {
        if (flowController.CompletedFlowCount < totalPairs) return;

        IsLevelComplete = true;
        bool hasNext    = currentLevel + 1 < GameData.Levels.Length;
        uiManager.ShowWin(hasNext);
    }

    public void NextLevel()
    {
        if (currentLevel + 1 < GameData.Levels.Length)
            LoadLevel(currentLevel + 1);
    }

    public void RestartLevel() => LoadLevel(currentLevel);
}