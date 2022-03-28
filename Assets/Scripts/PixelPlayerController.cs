using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PixelPlayerController : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;

    public Texture2D texture;
    [SerializeField]
    int rad = 150;

    SpriteRenderer sr;

    Vector2Int gravity = new Vector2Int(0, 1);

    //player
    [SerializeField]
    private int jumpPower = 30;
    [SerializeField]
    private int walkSpeed = 10;
    Vector2Int velocity;
    private bool tryToJump;
    private float inputX;
    //Image stats
    Vector2Int playerPixelPosition;
    //World stats
    private int maxVelocityX = 30;
    private int maxVelocityY = 30;

    PlayerBounds bounds;
    Bounds colliderBounds => collider.bounds;
    new Collider2D collider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);

        collider= GetComponent<Collider2D>();

        playerPixelPosition = GetPixelPosition(transform.position);

        StartCoroutine(PlayerUpdate());
    }

    private void Update()
    {
        GetInput();

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void LateUpdate()
    {

    }

    private IEnumerator PlayerUpdate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            UpdateBounds();
            UpdatePlayerPosition();

            UpdateDebug();
            
            DebugQuadDrawer.DrawLine(bounds.topLeftPixel,      bounds.topRightPixel,        Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.topRightPixel,     bounds.bottomRightPixel,     Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomRightPixel,  bounds.bottomLeftPixel,      Color.blue * .5f, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomLeftPixel,   bounds.topLeftPixel,         Color.blue * .5f, .01f);

            DebugQuadDrawer.DrawLine(bounds.topRightPixel, bounds.topRightPixel + new Vector2Int(8, -8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomRightPixel, bounds.bottomRightPixel+ new Vector2Int(8, 8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.bottomLeftPixel, bounds.bottomLeftPixel+ new Vector2Int(-8, 8), Color.white, .01f);
            DebugQuadDrawer.DrawLine(bounds.topLeftPixel, bounds.topLeftPixel + new Vector2Int(-8, -8), Color.white, .01f);

            DebugQuadDrawer.DrawLine(GetPixelPosition(transform.position), GetPixelPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Color.black, .01f);

            Debug.DrawRay(GetWorldPosition(playerPixelPosition), Vector3.up, Color.yellow, 0.01f);

            DebugQuadDrawer.DrawPixel(bounds.bottomPixel, Color.yellow);
            DebugQuadDrawer.DrawPixel(bounds.bottomRightPixel, Color.yellow);
            DebugQuadDrawer.DrawLine(bounds.bottomPixel, bounds.bottomRightPixel, Color.green);

            yield return new WaitForSeconds(.01f);
        }
    }

    private void UpdateBounds()
    {
        bounds.topRight = colliderBounds.max;
        bounds.bottomRight = colliderBounds.max + Vector3.down * colliderBounds.size.y;
        bounds.bottomLeft = colliderBounds.min;
        bounds.topLeft = colliderBounds.min + Vector3.up * colliderBounds.size.y;

        bounds.top = colliderBounds.center + Vector3.up * colliderBounds.extents.y;
        bounds.right = colliderBounds.center + Vector3.right * colliderBounds.extents.x;
        bounds.bottom = colliderBounds.center + Vector3.down * colliderBounds.extents.y;
        bounds.left = colliderBounds.center + Vector3.left * colliderBounds.extents.x;

        bounds.topRightPixel = GetPixelPosition(bounds.topRight, new Vector2Int(-1,1));
        bounds.bottomRightPixel = GetPixelPosition(bounds.bottomRight, new Vector2Int(-1, 0));
        bounds.bottomLeftPixel = GetPixelPosition(bounds.bottomLeft);
        bounds.topLeftPixel = GetPixelPosition(bounds.topLeft, new Vector2Int(0, 1));

        bounds.topPixel = GetPixelPosition(bounds.top, new Vector2Int(0, 1));
        bounds.rightPixel = GetPixelPosition(bounds.right, new Vector2Int(-1, 0));
        bounds.bottomPixel = GetPixelPosition(bounds.bottom);
        bounds.leftPixel = GetPixelPosition(bounds.left);
    }

    private void UpdateDebug()
    {

    }

    private void UpdatePlayerPosition()
    {
        velocity += gravity;

        if (tryToJump)
            TryJump();
        tryToJump = false;

        velocity.x = (int)Input.GetAxisRaw("Horizontal") * walkSpeed;

        CheckCollisionImage();
        CheckCollisionWorld();


        velocity.x = Mathf.Clamp(velocity.x, -maxVelocityX, maxVelocityX);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);

        playerPixelPosition += velocity/10;

        transform.position = GetWorldPosition(playerPixelPosition);
    }

    private void CheckCollisionImage()
    {
        int collisionMask = 0;

        if (OverlapPointPixel(bounds.bottomPixel, out Color col))
            collisionMask |= 1 << 3;

        DebugQuadDrawer.DrawPixel(bounds.bottomPixel + new Vector2Int(0, 1), Color.red);
        if(OverlapPointPixel(bounds.bottomPixel + new Vector2Int(0, 1), out Color c))
        {
            DebugQuadDrawer.DrawPixel(bounds.bottomPixel + new Vector2Int(0, 1), Color.green);
            velocity.y = 0;
            if (collisionMask == 1 << 3)
            {
                DebugQuadDrawer.DrawPixel(bounds.bottomPixel, Color.green);
                playerPixelPosition.y -= 1;
            }
        }
    }


    private void CheckCollisionWorld()
    {

    }
    private void TryJump()
    {
        velocity.y = -jumpPower;
    }


    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            tryToJump = true;
        inputX = Input.GetAxisRaw("Horizontal");
    }

    private Vector3 GetWorldPosition(Vector2Int playerPixelPosition)
    {
        var worldPosition = new Vector2((float)playerPixelPosition.x / 320 * 20 - 10,-((float)playerPixelPosition.y /288 * 18 - 9));
        return worldPosition;
    }

    private Vector2Int GetPixelPosition(Vector2 position)
    {
        return GetPixelPosition(position, Vector2Int.zero);
    }

    private Vector2Int GetPixelPosition(Vector2 position, Vector2Int pixelOffset)
    {
        var pixelPos = new Vector2(((position.x + 10) / 20) * 320, (-position.y + 9) / 18 * 288);
        pixelPos.x = Mathf.Round(pixelPos.x);
        pixelPos.y -= 1; //Todo: This is weird... it makes pixelposition on debug and this script match
        return Vector2Int.RoundToInt(pixelPos) + pixelOffset;
    }
    public bool RayCastPixel(Vector2Int start, Vector2Int end, out float distance)
    {
        int signedWidth = end.x - start.x;
        int signedHeight = end.y - start.y;

        int width = Mathf.Abs(signedWidth) + 1;
        int height = Mathf.Abs(signedHeight) + 1 ;

        int max = Mathf.Max(width, height);

        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Rect rectReadPicture = new Rect(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y), width, height);
        RenderTexture.active = renderTexture;

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        for (int i = 1; i < max; i++)
        {
            float delta = (float)i / max;
            int x = Mathf.RoundToInt(signedWidth * delta) + start.x;
            int y = Mathf.RoundToInt(signedHeight * delta) + start.y;

            var pixel = texture.GetPixel(x, y);
            if(pixel.a > .4f)
            {
                distance = Vector2Int.Distance(start, new Vector2Int(x, y));
                return true;
            }
        }
        distance = -1;
        return false;

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

    private bool OverlapArea(Vector2Int pixelPosition, int width, int height)
    {
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
    private bool OverlapPointPixel(Vector2Int pixelPosition, out Color color)
    {
        texture = new Texture2D(3, 3, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        RenderTexture.active = renderTexture;
        Rect rectReadPicture = new Rect(pixelPosition.x - 1, pixelPosition.y - 1, 3, 3);

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        var col = texture.GetPixel(1, 1);

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
        public Vector2 top, right, bottom, left;
        public Vector2Int topPixel, rightPixel, bottomPixel, leftPixel;
        public Vector2Int topRightPixel, bottomRightPixel, bottomLeftPixel, topLeftPixel;

    }
}
