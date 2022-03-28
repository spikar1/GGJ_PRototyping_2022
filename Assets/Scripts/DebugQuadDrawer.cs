using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DebugQuadDrawer : MonoBehaviour
{
    public Texture2D tex;
    public Material mat;

    static DebugQuadDrawer instance;
    public static DebugQuadDrawer Instance => instance;


    private void Start()
    {
        if (!instance)
            instance = this;
        if (instance != this)
            Destroy(this);

        tex = new Texture2D(320, 288, TextureFormat.ARGB32, false);
        tex.SetPixels(new Color[320 * 288]);
        tex.filterMode = FilterMode.Point;
        mat = GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex", tex);
    }

    private void Update()
    {
    }

    private static Vector2Int GetPixelPosition(Vector2 position)
    {
        return GetPixelPosition(position, Vector2Int.zero);
    }

    private static Vector2Int GetPixelPosition(Vector2 position, Vector2Int pixelOffset)
    {
        var pixelPos = new Vector2(((position.x + 10) / 20) * 320, (position.y + 9) / 18 * 288);
        pixelPos.x = Mathf.Round(pixelPos.x);
        return Vector2Int.RoundToInt(pixelPos) + pixelOffset;
    }

    public static void DrawLine(Vector2Int start, Vector2Int end, Color color, float duration = .01f)
    {
        end.y = -end.y -1;
        start.y = -start.y -1;

        int width = (int)(end.x - start.x);
        int height = (int)(end.y - start.y);

        int max = Mathf.Max(Mathf.Abs(width), Mathf.Abs(height));

        for (int i = 0; i < max; i++)
        {
            float delta = (float)i / max;
            int x = Mathf.RoundToInt(width * delta)+ start.x;
            int y = Mathf.RoundToInt(height * delta) + start.y;

            instance.tex.SetPixel(x, y, color);
            instance.StartCoroutine(ResetPixel(x, y, duration));
        }
        instance.tex.SetPixel(end.x, end.y, color);
        instance.tex.SetPixel(start.x, start.y, Color.magenta);
        instance.StartCoroutine(ResetPixel(end.x, end.y, duration));
        instance.tex.Apply();

    }

    public static void DrawPixel(Vector2Int position, Color color, float duration = .01f)
    {
        position.y = -position.y -1;

        instance.tex.SetPixel(position.x, position.y, color);
        instance.StartCoroutine(ResetPixel(position.x, position.y, duration));
        instance.tex.Apply();
    }

    private static IEnumerator ResetPixel(int x, int y, float duration)
    {
        yield return new WaitForSeconds(duration);
        instance.tex.SetPixel(x, y, new Color());
    }
}
