using UnityEngine;
using System.Collections;
using LibNoise;
using LibNoise.Generator;

public class NoiseGenerator {

	public float[,] GenerateNoise(int width, int height, ModuleBase module) {

		// Create noiseMap and fill it with samples from noise module
		float[,] noiseMap = new float[width, height];

		for (int y = 0; y < height - 1; y++) {
			for(int x = 0; x < width - 1; x++) {
				// Center frequency scaling, sample noise module and clamp output
				float sampleX = x - width / 2;
				float sampleY = y - height / 2;

				noiseMap[x, y] = (float )module.GetValue (sampleX, sampleY, 0);
				Mathf.Clamp(noiseMap[x, y], -1f, 1f);

				// Convert range from [-1, 1] to [0, 1] for now
				noiseMap [x, y] = (noiseMap [x, y] + 1f) / 2f;
			}
		}

		return noiseMap;

	}

	// Gets min and max value of samples and number of samples out of range [-1, 1]
	void NoiseMapTest(float[,] noiseMap) {
		float biggest = -99999f;
		float smallest = 99999f;
		int count = 0;

		for (int y = 0; y < noiseMap.Length - 1; y++) {
			for(int x = 0; x < noiseMap.Length - 1; x++) {
				if (noiseMap [x, y] > biggest)
					biggest = noiseMap [x, y];
				if (noiseMap [x, y] < smallest)
					smallest = noiseMap [x, y];
				
				if (noiseMap [x, y] < -1f || noiseMap [x, y] > 1f)
					count++;
			}
		}
	}
		
}
