using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    Text       levelLabel;
    GameObject winPanel;
    Text       winLabel;
    Button     nextButton;

    public void Init(GameManager gm)
    {
        gameManager = gm;
        Build();
    }

    void Build()
    {
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        var canvasGo = new GameObject("Canvas");
        var canvas   = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(800, 600);
        canvasGo.AddComponent<GraphicRaycaster>();

        levelLabel = MakeText(canvasGo, "LevelLabel", "Level 1 / 5",
            new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -12),
            new Vector2(320, 48), 28, TextAnchor.MiddleCenter);

        var restartBtn = MakeButton(canvasGo, "RestartBtn", "Restart",
            new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(16, 16), new Vector2(120, 44));
        restartBtn.onClick.AddListener(() => gameManager.RestartLevel());

        winPanel = new GameObject("WinPanel");
        winPanel.transform.SetParent(canvasGo.transform, false);
        var panelImg  = winPanel.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.80f);
        var panelRect = winPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.25f, 0.30f);
        panelRect.anchorMax = new Vector2(0.75f, 0.70f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        winLabel = MakeText(winPanel, "WinLabel", "Level Solved!",
            new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(280, 60), 34, TextAnchor.MiddleCenter, Color.yellow);

        nextButton = MakeButton(winPanel, "NextBtn", "Next Level",
            new Vector2(0.5f, 0.30f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(160, 48));
        nextButton.onClick.AddListener(() => gameManager.NextLevel());

        winPanel.SetActive(false);
    }

    public void ShowLevel(int level, int total)
    {
        if (levelLabel) levelLabel.text = $"Level  {level} / {total}";
    }

    public void ShowWin(bool hasNext)
    {
        winPanel.SetActive(true);
        nextButton.gameObject.SetActive(hasNext);
        winLabel.text = hasNext ? "Level Solved!" : "All Levels Complete!";
    }

    public void HideWin() => winPanel.SetActive(false);

    Font GetFont()
    {
        var f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (f == null) f = Resources.GetBuiltinResource<Font>("Arial.ttf");
        return f;
    }

    Text MakeText(GameObject parent, string goName, string content,
        Vector2 anchor, Vector2 pivot, Vector2 anchoredPos, Vector2 size,
        int fontSize = 22, TextAnchor align = TextAnchor.MiddleLeft, Color? color = null)
    {
        var go   = new GameObject(goName);
        go.transform.SetParent(parent.transform, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin        = anchor;
        rect.anchorMax        = anchor;
        rect.pivot            = pivot;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta        = size;
        var txt = go.AddComponent<Text>();
        txt.font      = GetFont();
        txt.text      = content;
        txt.fontSize  = fontSize;
        txt.alignment = align;
        txt.color     = color ?? Color.white;
        return txt;
    }

    Button MakeButton(GameObject parent, string goName, string label,
        Vector2 anchor, Vector2 pivot, Vector2 anchoredPos, Vector2 size)
    {
        var go   = new GameObject(goName);
        go.transform.SetParent(parent.transform, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin        = anchor;
        rect.anchorMax        = anchor;
        rect.pivot            = pivot;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta        = size;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.20f, 0.20f, 0.20f);
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.colors = new ColorBlock
        {
            normalColor      = new Color(0.20f, 0.20f, 0.20f),
            highlightedColor = new Color(0.35f, 0.35f, 0.35f),
            pressedColor     = new Color(0.12f, 0.12f, 0.12f),
            selectedColor    = new Color(0.25f, 0.25f, 0.25f),
            disabledColor    = new Color(0.20f, 0.20f, 0.20f, 0.5f),
            colorMultiplier  = 1f,
            fadeDuration     = 0.1f,
        };

        MakeText(go, "Label", label,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero,
            size, 20, TextAnchor.MiddleCenter);

        return btn;
    }
}