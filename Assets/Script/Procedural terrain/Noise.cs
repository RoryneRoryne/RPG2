using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    //method untuk return 2D float values
    public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        //2D float array
        float [,] noiseMap = new float [mapWidth,mapHeight];
        //berguna agar scale tidak akan error karena scale dibagi dengan 0
        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //berguna untuk sampling height value
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX,sampleY);
                //berguna untuk mengapplu perlin value ke noise map
                noiseMap [x,y] = perlinValue;
            }
        }
        return noiseMap;
    }
}
