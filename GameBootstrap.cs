using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    void Start()
    {
        SetupCamera();

        var gridManager    = new GameObject("GridManager")   .AddComponent<GridManager>();
        var flowController = new GameObject("FlowController").AddComponent<FlowController>();
        var uiManager      = new GameObject("UIManager")     .AddComponent<UIManager>();
        var gameManager    = new GameObject("GameManager")   .AddComponent<GameManager>();

        gameManager.Init(gridManager, flowController, uiManager);
        uiManager.Init(gameManager);
        gameManager.LoadLevel(0);
    }

    static void SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            var go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            cam = go.AddComponent<Camera>();
        }

        cam.orthographic     = true;
        cam.orthographicSize = 5f;
        cam.clearFlags       = CameraClearFlags.SolidColor;
        cam.backgroundColor  = new Color(0.07f, 0.07f, 0.07f);
        cam.transform.position = new Vector3(0f, 0f, -10f);
    }
}