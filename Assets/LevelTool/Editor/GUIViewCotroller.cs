using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIViewCotroller 
{
    public static GUIStyle CreateStyle(int fontSize, FontStyle fontStyle, Color bgColor, TextAnchor alignment)
    {
        return new GUIStyle
        {
            fontSize = fontSize,
            fontStyle = fontStyle,
            alignment = alignment,
            normal = { background = GUIViewCotroller.MakeTexture2D(2, 2, bgColor) }
        };
    }

    public static Texture2D MakeTexture2D(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(pix);
        tex.Apply();
        return tex;
    }
}
