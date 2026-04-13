using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int gridPos;
    public int dotColor  = -1;
    public int pipeColor = -1;

    SpriteRenderer bgRenderer;
    SpriteRenderer pipeCenter;
    SpriteRenderer pipeUp, pipeDown, pipeLeft, pipeRight;
    SpriteRenderer dotRenderer;

    static Sprite squareSprite;
    static Sprite circleSprite;

    public static void EnsureSprites()
    {
        if (squareSprite != null) return;
        squareSprite = GameData.CreateSquareSprite();
        circleSprite = GameData.CreateCircleSprite();
    }

    public void Init(Vector2Int pos)
    {
        gridPos = pos;
        name    = $"Cell_{pos.x}_{pos.y}";

        bgRenderer       = MakeSR("BG", squareSprite, Vector2.zero, Vector2.one * 0.92f, 0);
        bgRenderer.color = new Color(0.13f, 0.13f, 0.13f);

        float thick = 0.50f;
        float reach = 0.56f;
        float mid   = reach / 2f;

        pipeCenter = MakeSR("PC", circleSprite, Vector2.zero,          new Vector2(thick, thick), 1);
        pipeRight  = MakeSR("PR", squareSprite, new Vector2( mid,  0), new Vector2(reach, thick), 1);
        pipeLeft   = MakeSR("PL", squareSprite, new Vector2(-mid,  0), new Vector2(reach, thick), 1);
        pipeUp     = MakeSR("PU", squareSprite, new Vector2(0,  mid),  new Vector2(thick, reach), 1);
        pipeDown   = MakeSR("PD", squareSprite, new Vector2(0, -mid),  new Vector2(thick, reach), 1);

        ClearPipe();

        dotRenderer       = MakeSR("Dot", circleSprite, Vector2.zero, Vector2.one * 0.78f, 3);
        dotRenderer.color = Color.clear;
    }

    SpriteRenderer MakeSR(string goName, Sprite sprite, Vector2 localPos, Vector2 localScale, int sortOrder)
    {
        var go = new GameObject(goName);
        go.transform.SetParent(transform, false);
        go.transform.localPosition = new Vector3(localPos.x, localPos.y, 0f);
        go.transform.localScale    = new Vector3(localScale.x, localScale.y, 1f);
        var sr         = go.AddComponent<SpriteRenderer>();
        sr.sprite       = sprite;
        sr.sortingOrder = sortOrder;
        return sr;
    }

    public void SetDot(int colorIdx)
    {
        dotColor          = colorIdx;
        dotRenderer.color = GameData.PipeColors[colorIdx];
    }

    public void SetPipeDirectional(int colorIdx, bool up, bool down, bool left, bool right)
    {
        pipeColor = colorIdx;
        Color c  = GameData.PipeColors[colorIdx];
        Color pc = new Color(c.r * 0.85f, c.g * 0.85f, c.b * 0.85f, 1f);

        pipeCenter.color = pc;
        pipeUp.color     = up    ? pc : Color.clear;
        pipeDown.color   = down  ? pc : Color.clear;
        pipeLeft.color   = left  ? pc : Color.clear;
        pipeRight.color  = right ? pc : Color.clear;
    }

    public void SetPipe(int colorIdx) => SetPipeDirectional(colorIdx, false, false, false, false);

    public void ClearPipe()
    {
        pipeColor        = -1;
        pipeCenter.color = Color.clear;
        pipeUp.color     = Color.clear;
        pipeDown.color   = Color.clear;
        pipeLeft.color   = Color.clear;
        pipeRight.color  = Color.clear;
    }

    public void SetCompleted(bool completed)
    {
        if (dotColor < 0) return;
        var c = GameData.PipeColors[dotColor];
        dotRenderer.color = completed ? Color.Lerp(c, Color.white, 0.35f) : c;
    }
}