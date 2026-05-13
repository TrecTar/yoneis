using UnityEngine;

public static class Optimizer
{
    public static bool ShowMenu;
    public static bool ShowFPS;

    public static float deltaTime;

    public static float treeDistance = 500f;
    public static float detailDistance = 0f;
    public static float detailDensity = 0f;
    public static float treeBillboardDistance;
    public static float treeCrossFadeLength;
    public static int treeMaximumFullLODCount;
    public static float heightmapPixelError;
    public static int heightmapMaximumLOD;
    public static float basemapDistance;
    public static int masterTextureLimit;
    public static float renderDistance;

    public static int minFPS = 999;
    public static float avgFPS = 0f;

    static int fpsFrames = 0;
    static float fpsTimer = 0f;
    static int fpsAccum = 0;

    public static bool UIBlocked()
    {
        return ChatUI.IsVisible()
            || ConsoleWindow.IsVisible()
            || MainMenu.IsVisible();
    }

    public static void ResetRecommended()
    {
        treeDistance = 300f;
        treeBillboardDistance = 250f;
        treeCrossFadeLength = 5f;
        treeMaximumFullLODCount = 5;
        heightmapPixelError = 20f;
        heightmapMaximumLOD = 0;
        basemapDistance = 100f;
        detailDistance = 0f;
        detailDensity = 0f;
        masterTextureLimit = 0;
        renderDistance = 0f;
    }
    public static void Update()
    {
        // FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        int fps = Mathf.RoundToInt(1f / Time.deltaTime);

        if (fps < minFPS)
            minFPS = fps;

        fpsAccum += fps;
        fpsFrames++;

        fpsTimer += Time.deltaTime;

        if (fpsTimer >= 1f)
        {
            avgFPS = (float)fpsAccum / fpsFrames;

            fpsTimer = 0f;
            fpsFrames = 0;
            fpsAccum = 0;
        }

        // Toggles
        if (Input.GetKeyDown(KeyCode.F2))
            ShowMenu = !ShowMenu;

        if (Input.GetKeyDown(KeyCode.F3))
            ShowFPS = !ShowFPS;

        Apply();
    }

    public static void Apply()
    {
        if (Terrain.activeTerrain == null) return;

        Terrain t = Terrain.activeTerrain;

        t.treeDistance = treeDistance;
        t.detailObjectDistance = detailDistance;
        t.detailObjectDensity = detailDensity;
    }

    public static int GetFPS()
    {
        return Mathf.RoundToInt(1f / deltaTime);
    }
}