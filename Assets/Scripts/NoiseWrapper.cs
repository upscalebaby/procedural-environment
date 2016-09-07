using UnityEngine;
using System.Collections;
using LibNoise;

public class NoiseWrapper {

    public int id;
    public ModuleBase baseModule;
    public float scale, bias, falloff, falloffScale;

    public NoiseWrapper(ModuleBase baseModule) {
        this.baseModule = baseModule;
    }

    public float[,] GeneratHeightMap(int width, int height) {

        // Create noiseMap and fill it with samples from noise module
        float[,] heightMap = new float[width, height];

        for (int y = 0; y < height - 1; y++) {
            for(int x = 0; x < width - 1; x++) {
                // Center frequency scaling, sample noise module and clamp output
                float sampleX = x - width / 2;
                float sampleY = y - height / 2;

                heightMap[x, y] = (float )baseModule.GetValue (sampleX, sampleY, 0);
                heightMap[x, y] = heightMap[x, y] * scale + bias;
                Mathf.Clamp(heightMap[x, y], -1f, 1f);

                // Convert range from [-1, 1] to [0, 1] for now
                heightMap [x, y] = (heightMap [x, y] + 1f) / 2f;
            }
        }

        return heightMap;

    }
        
}
