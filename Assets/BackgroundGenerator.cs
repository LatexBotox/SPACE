using UnityEngine;
using System.Collections;

public class BackgroundGenerator : MonoBehaviour {
    Texture2D tex;

    public int passes;
    public float persistence;
    public int scale;

    void Start()
    {
        tex = new Texture2D(2048, 2048);
        GenerateTexture(tex, Time.realtimeSinceStartup);

        renderer.material.mainTexture = tex;
        tex.Apply();
    }

    void GenerateTexture(Texture2D tex, float seed)
    {
        int w = tex.width;
        int h = tex.height;
        float noise;


        Color[] pixels = new Color[w * h];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                noise = 0;
                for (int i = 0; i < passes; i++)
                    noise += (SimplexNoise.SeamlessNoise(x / (float)w, y / (float)h, scale * Mathf.Pow(2, i), scale * Mathf.Pow(2, i), seed))*Mathf.Pow(1/-persistence,i);

                noise = Mathf.Abs(noise);

                float c = Mathf.Sin(noise);

                noise = Mathf.Tan(noise/Mathf.PI*2);
                
                

                

                pixels[y * h + x] = new Color(c, c, c, noise);
            }
        }

        tex.SetPixels(pixels);
    }
}
