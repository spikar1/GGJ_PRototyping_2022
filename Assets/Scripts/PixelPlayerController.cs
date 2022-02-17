using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPlayerController : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;

    public Texture2D texture;
    [SerializeField]
    int pX;
    [SerializeField]
    int pY;
    [SerializeField]
    int rad = 150;

    SpriteRenderer sr;

    Vector3 gravity = new Vector2(0, -.01f);

    const float ratio = 10f / 9f;

    //player
    Vector3 velocity;
    private bool tryToJump;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);


        StartCoroutine(PlayerUpdate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            tryToJump = true;
    }

    private IEnumerator PlayerUpdate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            UpdatePlayerPosition();

            yield return new WaitForSeconds(.01f);
        }
    }

    private void UpdatePlayerPosition()
    {
        Vector3 newPos = transform.position;

        /*if (!OverlapArea(newPos + gravity, 16,16))
        else
            velocity = Vector3.zero;*/

        velocity += gravity;


        if(Linecast(newPos + new Vector3(-.5f, -.5f),newPos + new Vector3(.5f, -.5f)))
        {
            velocity.y = 0;
            if (tryToJump)
                velocity = new Vector3(velocity.x, .2f);

        }

        newPos += velocity;
        transform.position = newPos;
        tryToJump = false;
    }

    private void LateUpdate()
    {
        Vector2 pos = GetPixelPosition(transform.position);
        pX = (int)pos.x;
        pY = (int)pos.y;


    }

    private Vector2 GetPixelPosition(Vector3 position)
    {
        return new Vector2(((position.x + 10)/ 20) * 320, (-position.y + 9) / 18 * 288);
    }

    private bool Linecast(Vector3 pos, Vector3 dest)
    {
        Vector2 pixelPositionStart = GetPixelPosition(pos);
        Vector2 pixelPositionEnd = GetPixelPosition(dest);

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


        for (int i = 0; i < Mathf.Max(width, height); i++)
        {

            var lerp = Vector2.Lerp(pixelPositionStart, pixelPositionEnd, (float)i / (float)Mathf.Max(width, height));
            int x = (int)lerp.x;
            int y = (int)lerp.y;
            var col = texture.GetPixel(x,y);

            if (col.a > .4f)
                return true;
        }

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

    private bool OverlapPoint(Vector3 pos)
    {
        Vector2 pixelPosition = GetPixelPosition(pos);

        texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

        RenderTexture.active = renderTexture;
        Rect rectReadPicture = new Rect(pixelPosition.x, pixelPosition.y, 1, 1);

        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        var col = texture.GetPixel(0, 0);

        if (col.a > .4f)
            return true;
        else
            return false;


    }

    private void TestRenderTextureToTextureFunction()
    {

        RenderTexture.active = renderTexture;

        Rect rectReadPicture = new Rect(pX, pY, 256, 256);
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
}
