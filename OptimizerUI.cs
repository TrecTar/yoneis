using UnityEngine;

public static class OptimizerUI
{
    public static Rect window = new Rect(10f, 97f, 230f, 610f);

    static GUIStyle windowStyle;
    static GUIStyle headerStyle;
    static GUIStyle labelStyle;
    static GUIStyle valueStyle;
    static GUIStyle buttonStyle;
    static GUIStyle watermarkStyle;

    static Texture2D backgroundTexture;
    static Texture2D accentTexture;

    static GUIStyle fpsMainStyle;
    static GUIStyle fpsSubStyle;

    static bool initialized;

    static void Init()
    {
        if (initialized) return;

        initialized = true;

        backgroundTexture = MakeTex(new Color(0.08f, 0.08f, 0.08f, 0.96f));
        accentTexture = MakeTex(new Color(1f, 0.2f, 0.8f, 1f));

        windowStyle = new GUIStyle(GUI.skin.window);
        windowStyle.normal.background = backgroundTexture;
        windowStyle.border = new RectOffset(8, 8, 8, 8);
        windowStyle.padding = new RectOffset(10, 10, 10, 10);

        headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.richText = true;
        headerStyle.fontSize = 20;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.normal.textColor = Color.white;

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 11;
        labelStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);

        valueStyle = new GUIStyle(GUI.skin.box);
        valueStyle.alignment = TextAnchor.MiddleCenter;
        valueStyle.fontStyle = FontStyle.Bold;
        valueStyle.normal.textColor = Color.white;
        valueStyle.normal.background = accentTexture;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fontSize = 11;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = Color.white;
        buttonStyle.normal.background = accentTexture;
        buttonStyle.hover.background = accentTexture;

        watermarkStyle = new GUIStyle(GUI.skin.box);
        watermarkStyle.richText = true;
        watermarkStyle.alignment = TextAnchor.MiddleCenter;
        watermarkStyle.fontStyle = FontStyle.Bold;
        watermarkStyle.fontSize = 14;
        watermarkStyle.normal.textColor = Color.white;
        watermarkStyle.normal.background = backgroundTexture;
    }

    static Texture2D MakeTex(Color color)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return tex;
    }

    public static void Draw()
    {
        if (Optimizer.UIBlocked()) return;

        Init();

        DrawWatermark();
        DrawFPS();

        if (Optimizer.ShowMenu)
            GUI.Window(2, new Rect(10f, 97f, 230f, 610f), Ventana, "", windowStyle);
    }

    static void DrawWatermark()
    {
        GUI.Box(
            new Rect(10f, 5f, 230f, 30f),
            "<color=#FF85E7><b>RUST YONEIS</b></color>  <color=#AAAAAA>v0.5</color>",
            watermarkStyle
        );

        GUI.Box(
            new Rect(10f, 40f, 230f, 30f),
            "<color=#FF85E7><b>F2</b></color> Optimizer   <color=#FF85E7><b>F3</b></color> FPS",
            watermarkStyle
        );
    }

    static void DrawFPS()
    {
        if (!Optimizer.ShowFPS)
            return;

        if (fpsMainStyle == null)
        {
            fpsMainStyle = new GUIStyle(GUI.skin.label);
            fpsMainStyle.fontSize = 22;
            fpsMainStyle.fontStyle = FontStyle.Bold;
            fpsMainStyle.alignment = TextAnchor.MiddleCenter;
            fpsMainStyle.normal.textColor = Color.white;

            fpsSubStyle = new GUIStyle(GUI.skin.label);
            fpsSubStyle.fontSize = 10;
            fpsSubStyle.alignment = TextAnchor.MiddleCenter;
            fpsSubStyle.normal.textColor = new Color(1f, 0.4f, 0.85f);
        }

        int fps = Optimizer.GetFPS();

        GUI.Box(
            new Rect(245f, 5f, 130f, 55f),
            "",
            watermarkStyle
        );

        GUI.Label(
            new Rect(245f, 6f, 130f, 30f),
            fps + " FPS",
            fpsMainStyle
        );

        GUI.Label(
            new Rect(245f, 32f, 130f, 20f),
            "MIN " + Optimizer.minFPS +
            "   AVG " + Mathf.RoundToInt(Optimizer.avgFPS),
            fpsSubStyle
        );
    }

    static void SliderBlock(
        ref float y,
        string label,
        ref float value,
        float min,
        float max,
        System.Action<float> onChanged
    )
    {
        GUI.Label(new Rect(15f, y, 200f, 20f), label, labelStyle);

        GUI.Box(
            new Rect(15f, y + 18f, 55f, 20f),
            value.ToString("0.#"),
            valueStyle
        );

        float newValue = GUI.HorizontalSlider(
            new Rect(80f, y + 24f, 130f, 15f),
            value,
            min,
            max
        );

        if (newValue != value)
        {
            value = newValue;
            onChanged(newValue);
        }

        y += 42f;
    }

    static void SliderBlockInt(
        ref float y,
        string label,
        ref int value,
        float min,
        float max,
        System.Action<int> onChanged
    )
    {
        GUI.Label(new Rect(15f, y, 200f, 20f), label, labelStyle);

        GUI.Box(
            new Rect(15f, y + 18f, 55f, 20f),
            value.ToString(),
            valueStyle
        );

        int newValue = Mathf.RoundToInt(
            GUI.HorizontalSlider(
                new Rect(80f, y + 24f, 130f, 15f),
                value,
                min,
                max
            )
        );

        if (newValue != value)
        {
            value = newValue;
            onChanged(newValue);
        }

        y += 42f;
    }

    static void Ventana(int id)
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return;

        GUI.Label(
            new Rect(0f, 10f, 230f, 30f),
            "<color=#FF85E7>OPTIMIZER</color>",
            headerStyle
        );

        float y = 50f;

        SliderBlock(ref y, "Tree Distance", ref Optimizer.treeDistance, 250f, 2000f,
            delegate (float v)
            {
                t.treeDistance = v;
            });

        SliderBlock(ref y, "Tree Billboard Distance", ref Optimizer.treeBillboardDistance, 30f, 250f,
            delegate (float v)
            {
                t.treeBillboardDistance = v;
            });

        SliderBlock(ref y, "Tree Crossfade", ref Optimizer.treeCrossFadeLength, 5f, 50f,
            delegate (float v)
            {
                t.treeCrossFadeLength = v;
            });

        SliderBlockInt(ref y, "Tree LOD Count", ref Optimizer.treeMaximumFullLODCount, 5f, 100f,
            delegate (int v)
            {
                t.treeMaximumFullLODCount = v;
            });

        SliderBlock(ref y, "Heightmap Pixel Error", ref Optimizer.heightmapPixelError, 20f, 1f,
            delegate (float v)
            {
                t.heightmapPixelError = v;
            });

        SliderBlockInt(ref y, "Heightmap Maximum LOD", ref Optimizer.heightmapMaximumLOD, 1f, 0f,
            delegate (int v)
            {
                t.heightmapMaximumLOD = v;
            });

        SliderBlock(ref y, "Basemap Distance", ref Optimizer.basemapDistance, 100f, 1000f,
            delegate (float v)
            {
                t.basemapDistance = v;
            });

        SliderBlock(ref y, "Detail Distance", ref Optimizer.detailDistance, 0f, 200f,
            delegate (float v)
            {
                t.detailObjectDistance = v;
            });

        SliderBlock(ref y, "Detail Density", ref Optimizer.detailDensity, 0f, 200f,
            delegate (float v)
            {
                t.detailObjectDensity = v;
            });

        SliderBlockInt(ref y, "Texture Limit", ref Optimizer.masterTextureLimit, 6f, 0f,
            delegate (int v)
            {
                QualitySettings.masterTextureLimit = v;
            });

        SliderBlock(ref y, "Render Distance", ref Optimizer.renderDistance, 0f, 1f,
            delegate (float v)
            {
                global::render.distance = v;
            });

        if (GUI.Button(
            new Rect(15f, 525f, 200f, 28f),
            "CONFIG RECOMENDADA",
            buttonStyle))
        {
            Optimizer.ResetRecommended();
        }

        if (GUI.Button(
            new Rect(15f, 558f, 96f, 28f),
            "PASTO ON",
            buttonStyle))
        {
            t.detailObjectDistance = 150f;
            t.detailObjectDensity = 100f;
            ConsoleWindow.singleton.RunCommand("grass.on true");
        }

        if (GUI.Button(
            new Rect(119f, 558f, 96f, 28f),
            "PASTO OFF",
            buttonStyle))
        {
            t.detailObjectDistance = 0f;
            t.detailObjectDensity = 0f;
            ConsoleWindow.singleton.RunCommand("grass.on false");
        }
    }
}
