#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSpriteFromCamera : MonoBehaviour
{
    [SerializeField]
    bool disable = false;

    int width = 320;
    int height = 288;
    int depth = 1;

    Camera cam;

    public SpriteRenderer spriteRenderer;
    PolygonCollider2D pol;

    [SerializeField]
    GameObject cheapQuad, expensiveSprite;

    bool allowFreeToggle;

    PlayModeManager.PlayMode? lastKnownMode = null;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (disable)
            return;

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle || allowFreeToggle)
                PlayModeManager.Instance.SwitchModes();
        }

        if (PlayModeManager.Instance.CurrentMode != lastKnownMode)
            OnTogglePlayMode();
    }

    public void OnTogglePlayMode()
    {
        if (PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Platform)
        {
            cheapQuad.SetActive(false);
            expensiveSprite.SetActive(true);

            UpdateSprite();
        }
        else if (PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
        {
            cheapQuad.SetActive(true);
            expensiveSprite.SetActive(false);
        }

        lastKnownMode = PlayModeManager.Instance.CurrentMode;
    }


    void UpdateSprite()
    {
        spriteRenderer.sprite = CaptureScreen();
    }

    public Sprite CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        renderTexture.antiAliasing = 1;
        renderTexture.useMipMap = false;
        //renderTexture.
        renderTexture.filterMode = FilterMode.Point;
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

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero,16);

        return sprite;
    }
}
