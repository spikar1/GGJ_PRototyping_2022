using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSpriteFromCamera : MonoBehaviour
{
    int height = 1024;
    int width = 1024;
    int depth = 1;

    Camera cam;

    public SpriteRenderer spriteRenderer;
    PolygonCollider2D pol;

    [SerializeField]
    GameObject cheapQuad, expensiveSprite;
    [SerializeField]
    PlatformerPlayer player;

    [SerializeField]
    bool allowFreeToggle;

    private void Start()
    {
        player = FindObjectOfType<PlatformerPlayer>();
        player.Freeze();
        cam = GetComponent<Camera>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && (!expensiveSprite.activeSelf || allowFreeToggle))
        {
            TogglePlayMode();
        }
    }

    public void TogglePlayMode()
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

    private void UpdateCollider()
    {
        //todo: Implement bloom to get better colliders at straight angles.

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
        renderTexture.antiAliasing = 0;
        Rect rect = new Rect(0, 0, width, height);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        cam.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        return sprite;
    }
}