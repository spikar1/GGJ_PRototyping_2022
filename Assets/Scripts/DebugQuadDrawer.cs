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
        tex.SetPixel(100, 100, Random.ColorHSV());
        tex.Apply();
    }

    private static Vector2 GetPixelPosition(Vector2 position)
    {
        var pixelPos = new Vector2(((position.x + 10) / 20) * 320, (position.y + 9) / 18 * 288);
        pixelPos.x = Mathf.Round(pixelPos.x);
        pixelPos.y = Mathf.Round(pixelPos.y);
        return pixelPos;
    }

    public static void DrawLine(Vector2 a, Vector2 b, Color color, float duration)
    {
        Vector2 pixelPositionStart = GetPixelPosition(a);
        Vector2 pixelPositionEnd = GetPixelPosition(b);

        float maxDistance = Vector2.Distance(a, b);

        int width = (int)Mathf.Abs(pixelPositionStart.x - pixelPositionEnd.x);
        int height = (int)Mathf.Abs(pixelPositionStart.y - pixelPositionEnd.y);

        if (width == 0)
            width = 1;
        if (height == 0)
            height = 1;



        for (int i = 0; i < Mathf.Max(width, height); i++)
        {
            float delta = (float)i / (float)Mathf.Max(width, height);
            var lerp = Vector2.Lerp(pixelPositionStart, pixelPositionEnd, delta);
            int x = (int)lerp.x;
            int y = (int)lerp.y;

            instance.tex.SetPixel(x, y, color);
            instance.StartCoroutine(ResetPixel(x, y, duration));
        }
        instance.tex.Apply();

    }

    private static IEnumerator ResetPixel(int x, int y, float duration)
    {
        yield return new WaitForSeconds(duration);
        instance.tex.SetPixel(x, y, new Color());
    }
}
