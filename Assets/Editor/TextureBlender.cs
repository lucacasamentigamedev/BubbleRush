/*using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureOverlayWithPPU : EditorWindow
{
    Texture2D baseTexture;
    Texture2D overlayTexture;

    [MenuItem("Tools/Overlay Textures (PPU Aware)")]
    public static void ShowWindow()
    {
        GetWindow<TextureOverlayWithPPU>("Overlay Textures (PPU Aware)");
    }

    void OnGUI()
    {
        GUILayout.Label("Sovrapponi una texture sopra un'altra mantenendo scala PPU", EditorStyles.boldLabel);

        baseTexture = (Texture2D)EditorGUILayout.ObjectField("Texture base (sotto)", baseTexture, typeof(Texture2D), false);
        overlayTexture = (Texture2D)EditorGUILayout.ObjectField("Overlay (sopra)", overlayTexture, typeof(Texture2D), false);

        if (GUILayout.Button("Crea texture sovrapposta con PPU"))
        {
            if (baseTexture && overlayTexture)
                OverlayTexturesRespectingPPU(baseTexture, overlayTexture);
            else
                Debug.LogWarning("Assicurati di assegnare entrambe le texture.");
        }
    }

    void OverlayTexturesRespectingPPU(Texture2D baseTex, Texture2D overlayTex)
    {
        string basePath = AssetDatabase.GetAssetPath(baseTex);
        string overlayPath = AssetDatabase.GetAssetPath(overlayTex);

        var baseImporter = AssetImporter.GetAtPath(basePath) as TextureImporter;
        var overlayImporter = AssetImporter.GetAtPath(overlayPath) as TextureImporter;

        if (baseImporter == null || overlayImporter == null)
        {
            Debug.LogError("Impossibile leggere gli importer delle texture.");
            return;
        }

        float basePPU = baseImporter.spritePixelsPerUnit;
        float overlayPPU = overlayImporter.spritePixelsPerUnit;

        float scale = basePPU / overlayPPU;

        // Calcola nuove dimensioni dell'overlay per rispettare il PPU
        int scaledOverlayWidth = Mathf.RoundToInt(overlayTex.width * scale);
        int scaledOverlayHeight = Mathf.RoundToInt(overlayTex.height * scale);

        // Ridimensiona overlay
        Texture2D resizedOverlay = ResizeTexture(overlayTex, scaledOverlayWidth, scaledOverlayHeight);

        int width = baseTex.width;
        int height = baseTex.height;

        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Copia la base
        result.SetPixels(baseTex.GetPixels());

        // Calcola centro
        int offsetX = (width - scaledOverlayWidth) / 2;
        int offsetY = (height - scaledOverlayHeight) / 2;

        Color[] overlayPixels = resizedOverlay.GetPixels();

        for (int y = 0; y < scaledOverlayHeight; y++)
        {
            for (int x = 0; x < scaledOverlayWidth; x++)
            {
                int resultX = x + offsetX;
                int resultY = y + offsetY;

                if (resultX >= 0 && resultX < width && resultY >= 0 && resultY < height)
                {
                    Color baseColor = result.GetPixel(resultX, resultY);
                    Color overlayColor = overlayPixels[y * scaledOverlayWidth + x];
                    Color finalColor = Color.Lerp(baseColor, overlayColor, overlayColor.a);
                    result.SetPixel(resultX, resultY, finalColor);
                }
            }
        }

        result.Apply();

        // Salva la texture
        string savePath = "Assets/OverlayResult2.png";
        File.WriteAllBytes(savePath, result.EncodeToPNG());
        AssetDatabase.Refresh();

        // Imposta il PPU corretto (quello della base)
        SetTexturePPU(savePath, basePPU);

        Debug.Log($"Texture sovrapposta salvata con PPU = {basePPU} in: {savePath}");
    }

    void SetTexturePPU(string path, float ppu)
    {
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = ppu;
            importer.alphaIsTransparency = true;
            importer.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);
        RenderTexture.active = rt;

        Texture2D newTex = new Texture2D(newWidth, newHeight);
        newTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTex.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return newTex;
    }
}
*/

using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureOverlayWithPPU : EditorWindow
{
    Texture2D baseTexture;
    Texture2D overlayTexture;
    int offsetX = 0;
    int offsetY = 0;
    float overlayZoom = 1f;  // Zoom manuale solo per overlay

    [MenuItem("Tools/Overlay Textures (PPU + Zoom)")]
    public static void ShowWindow()
    {
        GetWindow<TextureOverlayWithPPU>("Overlay Textures (PPU + Zoom)");
    }

    void OnGUI()
    {
        GUILayout.Label("Sovrapponi una texture con PPU e scala personalizzata", EditorStyles.boldLabel);

        baseTexture = (Texture2D)EditorGUILayout.ObjectField("Texture base (sotto)", baseTexture, typeof(Texture2D), false);
        overlayTexture = (Texture2D)EditorGUILayout.ObjectField("Overlay (sopra)", overlayTexture, typeof(Texture2D), false);

        GUILayout.Space(10);
        GUILayout.Label("Offset dell'overlay (rispetto al centro della base):");
        offsetX = EditorGUILayout.IntSlider("Offset X", offsetX, -500, 500);
        offsetY = EditorGUILayout.IntSlider("Offset Y", offsetY, -500, 500);

        GUILayout.Space(10);
        overlayZoom = EditorGUILayout.Slider("Zoom manuale overlay", overlayZoom, 0.1f, 5f);

        GUILayout.Space(10);
        if (GUILayout.Button("Crea texture sovrapposta"))
        {
            if (baseTexture && overlayTexture)
                OverlayTexturesWithZoom(baseTexture, overlayTexture, offsetX, offsetY, overlayZoom);
            else
                Debug.LogWarning("Assegna entrambe le texture.");
        }
    }

    void OverlayTexturesWithZoom(Texture2D baseTex, Texture2D overlayTex, int offsetX, int offsetY, float overlayZoom)
    {
        string basePath = AssetDatabase.GetAssetPath(baseTex);
        string overlayPath = AssetDatabase.GetAssetPath(overlayTex);

        var baseImporter = AssetImporter.GetAtPath(basePath) as TextureImporter;
        var overlayImporter = AssetImporter.GetAtPath(overlayPath) as TextureImporter;

        if (baseImporter == null || overlayImporter == null)
        {
            Debug.LogError("Impossibile leggere gli importer.");
            return;
        }

        float basePPU = baseImporter.spritePixelsPerUnit;
        float overlayPPU = overlayImporter.spritePixelsPerUnit;

        // Calcola scala relativa e applica zoom manuale
        float scale = (basePPU / overlayPPU) * overlayZoom;

        int scaledOverlayWidth = Mathf.RoundToInt(overlayTex.width * scale);
        int scaledOverlayHeight = Mathf.RoundToInt(overlayTex.height * scale);

        Texture2D resizedOverlay = ResizeTexture(overlayTex, scaledOverlayWidth, scaledOverlayHeight);

        int width = baseTex.width;
        int height = baseTex.height;

        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
        result.SetPixels(baseTex.GetPixels());

        int startX = (width - scaledOverlayWidth) / 2 + offsetX;
        int startY = (height - scaledOverlayHeight) / 2 + offsetY;

        Color[] overlayPixels = resizedOverlay.GetPixels();

        for (int y = 0; y < scaledOverlayHeight; y++)
        {
            for (int x = 0; x < scaledOverlayWidth; x++)
            {
                int resultX = x + startX;
                int resultY = y + startY;

                if (resultX >= 0 && resultX < width && resultY >= 0 && resultY < height)
                {
                    Color baseColor = result.GetPixel(resultX, resultY);
                    Color overlayColor = overlayPixels[y * scaledOverlayWidth + x];
                    Color finalColor = Color.Lerp(baseColor, overlayColor, overlayColor.a);
                    result.SetPixel(resultX, resultY, finalColor);
                }
            }
        }

        result.Apply();

        string savePath = "Assets/OverlayResult.png";
        File.WriteAllBytes(savePath, result.EncodeToPNG());
        AssetDatabase.Refresh();

        SetTexturePPU(savePath, basePPU);

        Debug.Log($"✅ Texture salvata con zoom {overlayZoom} e offset ({offsetX}, {offsetY}) → {savePath}");
    }

    void SetTexturePPU(string path, float ppu)
    {
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = ppu;
            importer.alphaIsTransparency = true;
            importer.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);
        RenderTexture.active = rt;

        Texture2D newTex = new Texture2D(newWidth, newHeight);
        newTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTex.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return newTex;
    }
}
