using UnityEngine;

public static class EditorHelper
{
    public static GUIStyle CreateStyle(int fontSize, FontStyle fontStyle, Color bgColor, TextAnchor alignment)
    {
        return new GUIStyle
        {
            fontSize = fontSize,
            fontStyle = fontStyle,
            alignment = alignment,
            normal = { background = MakeTexture(2, 2, bgColor) }
        };
    }

    public static Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pixels);
        result.Apply();
        return result;
    }
}
