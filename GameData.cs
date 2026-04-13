using UnityEngine;

public static class GameData
{
    public static readonly Color[] PipeColors = new Color[]
    {
        new Color(0.95f, 0.20f, 0.20f),
        new Color(0.15f, 0.85f, 0.15f),
        new Color(0.20f, 0.45f, 0.95f),
        new Color(0.95f, 0.90f, 0.10f),
        new Color(0.75f, 0.20f, 0.80f),
        new Color(1.00f, 0.55f, 0.10f),
    };

    public struct DotPair
    {
        public int colorIdx;
        public Vector2Int a, b;
        public DotPair(int c, int ax, int ay, int bx, int by)
        {
            colorIdx = c;
            a = new Vector2Int(ax, ay);
            b = new Vector2Int(bx, by);
        }
    }

    public struct Level
    {
        public int size;
        public DotPair[] pairs;
        public Level(int s, params DotPair[] p) { size = s; pairs = p; }
    }

    public static readonly Level[] Levels = new Level[]
    {
        new Level(5,
            new DotPair(0,  0,4,  1,0),
            new DotPair(1,  2,4,  1,1),
            new DotPair(2,  2,3,  2,0),
            new DotPair(3,  4,4,  3,1),
            new DotPair(5,  4,3,  3,0)
        ),
        new Level(5,
            new DotPair(0,  0,0,  2,1),
            new DotPair(1,  1,1,  2,2),
            new DotPair(2,  0,1,  4,0),
            new DotPair(3,  0,4,  4,1)
        ),
        new Level(5,
            new DotPair(0,  2,2,  3,3),
            new DotPair(1,  3,4,  3,0),
            new DotPair(2,  2,4,  0,0),
            new DotPair(3,  1,4,  0,1),
            new DotPair(5,  3,1,  2,0)
        ),
        new Level(5,
            new DotPair(0,  0,3,  3,4),
            new DotPair(1,  4,4,  0,0),
            new DotPair(2,  1,0,  3,1),
            new DotPair(3,  2,2,  2,0)
        ),
        new Level(5,
            new DotPair(0,  3,4,  2,0),
            new DotPair(1,  4,4,  3,3),
            new DotPair(2,  2,3,  4,1),
            new DotPair(3,  1,3,  4,0)
        ),
    };

    public static Sprite CreateSquareSprite()
    {
        var tex    = new Texture2D(4, 4) { filterMode = FilterMode.Point };
        var pixels = new Color[16];
        for (int i = 0; i < 16; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
    }

    public static Sprite CreateCircleSprite(int res = 64)
    {
        var tex    = new Texture2D(res, res) { filterMode = FilterMode.Bilinear };
        float c    = (res - 1) / 2f;
        float r    = c * 0.88f;
        var pixels = new Color[res * res];
        for (int y = 0; y < res; y++)
        for (int x = 0; x < res; x++)
        {
            float d = Mathf.Sqrt((x - c) * (x - c) + (y - c) * (y - c));
            float a = Mathf.Clamp01(r - d + 1f);
            pixels[y * res + x] = new Color(1, 1, 1, a);
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f), (float)res);
    }
}