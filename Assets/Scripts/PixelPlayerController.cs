using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class PixelPlayerController : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;

    public Texture2D texture;
    [SerializeField]
    int rad = 150;

    SpriteRenderer sr;

    Vector3 gravity = new Vector2(0, -.01f);

    const float ratio = 10f / 9f;

    //player
    Vector3 velocity;
    private bool tryToJump;
    private float inputX;
    //Image stats
    Vector2Int playerPixelPosition;
    //World stats



    PlayerBounds bounds;
    Bounds colliderBounds => collider.bounds;
    new Collider2D collider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);

        collider= GetComponent<Collider2D>();

        StartCoroutine(PlayerUpdate());
    }

    private void Update()
    {
        GetInput();


        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator PlayerUpdate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            UpdateBounds();
            UpdatePlayerPosition();

            UpdateDebug();
            //Debug.DrawLine(bounds.topLeft,      bounds.topRight,        Color.blue, 1);
            //Debug.DrawLine(bounds.topRight,     bounds.bottomRight,     Color.blue, 1);
            //Debug.DrawLine(bounds.bottomRight,  bounds.bottomLeft,      Color.blue, 1);
            //Debug.DrawLine(bounds.bottomLeft,   bounds.topLeft,         Color.blue, 1);
            
            DebugQuadDrawer.DrawLine(bounds.topLeftPixel,      bounds.topRightPixel,        Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.topRightPixel,     bounds.bottomRightPixel,     Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomRightPixel,  bounds.bottomLeftPixel,      Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomLeftPixel,   bounds.topLeftPixel,         Color.blue * .5f, .01f);

            DebugQuadDrawer.DrawLine(bounds.topRightPixel, bounds.topRightPixel + new Vector2Int(8, -8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomRightPixel, bounds.bottomRightPixel+ new Vector2Int(8, 8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomLeftPixel, bounds.bottomLeftPixel+ new Vector2Int(-8, 8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.topLeftPixel, bounds.topLeftPixel + new Vector2Int(-8, -8), Color.white, .01f);

            DebugQuadDrawer.DrawLine(GetPixelPosition(transform.position), GetPixelPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Color.black, .01f);

            yield return new WaitForSeconds(.01f);
        }
    }

    private void UpdateBounds()
    {
        bounds.topRight = colliderBounds.max;
        bounds.bottomRight = colliderBounds.max + Vector3.down * colliderBounds.size.y;
        bounds.bottomLeft = colliderBounds.min;
        bounds.topLeft = colliderBounds.min + Vector3.up * colliderBounds.size.y;

        bounds.topRightPixel = GetPixelPosition(bounds.topRight, new Vector2Int(-1,1));
        bounds.bottomRightPixel = GetPixelPosition(bounds.bottomRight, new Vector2Int(-1, 0));
        bounds.bottomLeftPixel = GetPixelPosition(bounds.bottomLeft);
        bounds.topLeftPixel = GetPixelPosition(bounds.topLeft, new Vector2Int(0, 1));
    }

    private void UpdateDebug()
    {

    }

    private void UpdatePlayerPosition()
    {
        Vector3 newPos = transform.position;

        velocity += gravity;

        CheckCollisionImage(velocity);
        CheckCollisionWorld(velocity);

        if (tryToJump)
            TryJump();

        newPos += velocity;
        //transform.position = newPos;
        tryToJump = false;
    }

    private void CheckCollisionImage(Vector3 velocity)
    {

    }

    private void CheckCollisionWorld(Vector3 velocity)
    {

    }
    private void TryJump()
    {
        throw new NotImplementedException();
    }


    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            tryToJump = true;
        inputX = Input.GetAxisRaw("Horizontal");
    }

    private void LateUpdate()
    {
        Vector2 pos = GetPixelPosition(transform.position);
        playerPixelPosition.x = (int)pos.x;
        playerPixelPosition.y = (int)pos.y;
    }
    private Vector2Int GetPixelPosition(Vector2 position)
    {
        return GetPixelPosition(position, Vector2Int.zero);
    }

    private Vector2Int GetPixelPosition(Vector2 position, Vector2Int pixelOffset)
    {
        var pixelPos = new Vector2(((position.x + 10) / 20) * 320, (-position.y + 9) / 18 * 288);
        pixelPos.x = Mathf.Round(pixelPos.x);
        return Vector2Int.RoundToInt(pixelPos) + pixelOffset;
    }

    private bool Raycast(Vector3 pos, Vector3 dest, out float distance)
    {
        Vector2 pixelPositionStart = GetPixelPosition(pos);
        Vector2 pixelPositionEnd = GetPixelPosition(dest);

        float maxDistance = Vector2.Distance(pos, dest);

        int width = (int)Mathf.Abs(pixelPositionStart.x - pixelPositionEnd.x);
        int height = (int)Mathf.Abs(pixelPositionStart.y - pixelPositionEnd.y);

        if (width == 0)
            width = 1;
        if (height == 0)
            height = 1;

        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Rect rectReadPicture = new Rect(Mathf.Min(pixelPositionStart.x, pixelPositionEnd.x), Mathf.Min(pixelPositionStart.y, pixelPositionEnd.y), width, height);
        RenderTexture.active = renderTexture;

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();


        for (int i = 0; i < Mathf.Sqrt(Mathf.Pow(width,2) + Mathf.Pow(height, 2)) ; i++)
        {
            float delta = (float)i / (float)Mathf.Max(width, height);
            var lerp = Vector2.Lerp(pixelPositionStart, pixelPositionEnd, delta);
            int x = (int)lerp.x;
            int y = (int)lerp.y;
            var col = texture.GetPixel(x,y);


            if (col.a > .4f)
            {
                distance = maxDistance * delta;

                return true;
            }
        }
        distance = float.MaxValue;
        return false;
    }

    private bool OverlapArea(Vector3 pos, int width, int height)
    {
        Vector2 pixelPosition = GetPixelPosition(pos);
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Rect rectReadPicture = new Rect(pixelPosition.x - width/2, pixelPosition.y-width/2, width, height);
        RenderTexture.active = renderTexture;

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var col = texture.GetPixel(x, y);
                if(col.a > .4f)
                    return true;
            }
        }

        return false;
    }

    private bool OverlapPoint(Vector3 pos, out Color color)
    {
        Vector2 pixelPosition = GetPixelPosition(pos);

        texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

        RenderTexture.active = renderTexture;
        Rect rectReadPicture = new Rect(pixelPosition.x, pixelPosition.y, 1, 1);

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        var col = texture.GetPixel(0, 0);

        color = col;

        if (col.a > .4f)
            return true;
        else
            return false;


    }

    private void TestRenderTextureToTextureFunction()
    {

        RenderTexture.active = renderTexture;

        Rect rectReadPicture = new Rect(playerPixelPosition.x, playerPixelPosition.y, 256, 256);
        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();


        float t = Time.realtimeSinceStartup;
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                texture.GetPixel(x, y);
                //Debug.DrawLine(new Vector3(x, y), new Vector3(x + 1, y + 1), texture.GetPixel(x, y));
                if (Time.realtimeSinceStartup - t > .1f)
                    throw new System.Exception("DOON DOON DOON");
            }
        }


        RenderTexture.active = null;
    }

    struct PlayerBounds
    {
        public Vector2 topRight, bottomRight, bottomLeft, topLeft;
        public Vector2Int topRightPixel, bottomRightPixel, bottomLeftPixel, topLeftPixel;

    }
}
