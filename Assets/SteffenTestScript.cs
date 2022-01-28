using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteffenTestScript : MonoBehaviour
{
    public Camera camera;
    int height = 1024*4;
    int width = 1024*4;
    int depth = 1;

    public SpriteRenderer spriteRenderer;
    PolygonCollider2D pol;

    [SerializeField]
    GameObject cheapQuad, expensiveSprite;
    [SerializeField]
    Player player;

    private void Awake()
    {
        player.Freeze();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            cheapQuad.SetActive(!cheapQuad.activeSelf);
            expensiveSprite.SetActive(!expensiveSprite.activeSelf);
            if (expensiveSprite.activeSelf)
            {
                UpdateSprite();
                UpdateCollider();
                player.UnFreeze();
            }
            else
                player.Freeze();
        }
    }

    private void UpdateCollider()
    {
        if (pol)
            Destroy(pol);
        pol = spriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
    }

    void UpdateSprite()
    {
        spriteRenderer.sprite = CaptureScreen();
    }

    public Sprite CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        Rect rect = new Rect(0, 0, width, height);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        GetComponent<Camera>().targetTexture = renderTexture;
        GetComponent<Camera>().Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        return sprite;
    }
}