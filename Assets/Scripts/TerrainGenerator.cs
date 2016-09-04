using UnityEngine;
using System.Collections;
using LibNoise.Unity;

public class TerrainGenerator : MonoBehaviour {
	public int width;
	public int height;
	public UnityEngine.Gradient gradient;

	float frequency = 0.1f;
	float lacunarity = 2f;
	float persistence = 0.5f;
	int octaves = 4;
	int seed = 0;

	GameObject terrainMesh;
	NoiseType type;

	// Use this for initialization
	void Start () {
		GenerateTerrain ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateTerrain() {
		// Generate noiseMap
		NoiseGenerator noiseGenerator = new NoiseGenerator ();
		float[,] noiseMap = noiseGenerator.GenerateNoise (type, width, height, frequency, lacunarity, persistence, octaves, seed);

		// Generate texture from noiseMap
		Texture2D texture = GenerateTexture (noiseMap);

		// Generate mesh if there is none
		if(terrainMesh == null) {
			terrainMesh = GameObject.CreatePrimitive (PrimitiveType.Plane);
		}

		// Apply texture to mesh
		terrainMesh.GetComponent<Renderer> ().sharedMaterial.mainTexture = texture;

		// Scale plane to width and height
		terrainMesh.GetComponent<Renderer> ().transform.localScale = new Vector3 (width , 1, height);
	}

	Texture2D GenerateTexture(float[,] noiseMap) {
		Texture2D texture = new Texture2D (width, height);

		// Translate noiseMap coordinates to pixels on texture
		for (int y = 0; y < height - 1; y++) {
			for(int x = 0; x < width - 1; x++) {
				Color color = Color.Lerp (Color.black, Color.white, noiseMap [x, y]);
				texture.SetPixel (x, y, color);
			}
		}

		texture.Apply ();
		return texture;
	}

	public void SetNoiseType(NoiseType type) {
		this.type = type;
	}

	public void SetFrequency(float input) {
		this.frequency = input;
	}

	public void SetLacunarity(float input) {
		this.lacunarity = input;
	}

	public void SetPersistence(float input) {
		this.persistence = input;
	}

	public void SetOctaves(float input) {
		this.octaves = (int )input;
	}

	public void SetSeed(float input) {
		this.seed = (int )input;
	}
}
