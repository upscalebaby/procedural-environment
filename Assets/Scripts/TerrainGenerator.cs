using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Operator;
using LibNoise.Unity.Generator;

public class TerrainGenerator : MonoBehaviour {
	public int width;
	public int height;
	public UnityEngine.Gradient gradient;

	private NoiseGenerator noiseGenerator;
	private NoiseType noiseType;

	private ModuleBase currentModule;
	private Perlin perlinModule;
	private RiggedMultifractal ridgedModule;
	private Billow billowModule;

	private ScaleBias scaleBiasModule;
	private Select selectModule;

	private GameObject terrainMesh;

	// Init values for modules
	private float frequency = 0.1f;
	private float lacunarity = 2f;
	private float persistence = 0.5f;
	private int octaves = 4;
	private int seed = 0;

	private float scale = 0.125f;
	private float bias = -0.75f;

	// Use this for initialization
	void Start () {
		noiseGenerator = new NoiseGenerator();
		perlinModule = new Perlin(frequency, lacunarity, persistence, octaves, seed, QualityMode.High);
		ridgedModule = new RiggedMultifractal(frequency, lacunarity, octaves, seed, QualityMode.High);
		billowModule = new Billow(frequency, lacunarity, persistence, octaves, seed, QualityMode.High);

		scaleBiasModule = new ScaleBias(scale, bias, billowModule);
		//selectModule = new Select(billowModule, ridgedModule, perlinModule);

		GenerateTerrain ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateTerrain() {
		ModuleBase noiseModule;

		switch(noiseType) {
			case NoiseType.Perlin:
				noiseModule = perlinModule;
				break;
			case NoiseType.RiggedMultifractal:
				noiseModule = ridgedModule;
				break;
			case NoiseType.Billow:
				noiseModule = scaleBiasModule;
				break;
			default:
				noiseModule = perlinModule;
				break;
		}

		// Generate noiseMap
		float[,] noiseMap = noiseGenerator.GenerateNoise (width, height, noiseModule);

		// Generate texture from noiseMap
		Texture2D texture = GenerateTexture (noiseMap);

		// Generate mesh if there is none
		if(terrainMesh == null) {
			terrainMesh = GameObject.CreatePrimitive (PrimitiveType.Plane);
		}

		// Apply texture to mesh
		terrainMesh.GetComponent<Renderer> ().sharedMaterial.mainTexture = texture;

		// Scale mesh to width and height
		terrainMesh.GetComponent<Renderer> ().transform.localScale = new Vector3 (width , 1, height);
	}

	Texture2D GenerateTexture(float[,] noiseMap) {
		Texture2D texture = new Texture2D (width, height);

		// Compute color for each pixel of texture
		for (int y = 0; y < height - 1; y++) {
			for(int x = 0; x < width - 1; x++) {
				Color color = gradient.Evaluate (noiseMap [x, y]);
				texture.SetPixel (x, y, color);
			}
		}

		texture.Apply ();
		return texture;
	}

	// Setters for UI elements
	public void SetNoiseType(NoiseType type) {
		this.noiseType = type;

	}

	// TODO: Make a reference based on a switch, then simply set this refernce? No we tried thath right?
	public void SetFrequency(float input) {
		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.Frequency = input;
				break;
			case NoiseType.RiggedMultifractal:
				ridgedModule.Frequency = input;
				break;
			case NoiseType.Perlin:
				perlinModule.Frequency = input;
				break;
		}
	}

	public void SetLacunarity(float input) {
		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.Lacunarity = input;
				break;
			case NoiseType.RiggedMultifractal:
				ridgedModule.Lacunarity = input;
				break;
			case NoiseType.Perlin:
				perlinModule.Lacunarity = input;
				break;
		}
	}

	public void SetPersistence(float input) {
		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.Persistence = input;
				break;
			case NoiseType.Perlin:
				perlinModule.Persistence = input;
				break;
		}
	}

	public void SetOctaves(float i) {
		int input = (int )i;

		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.OctaveCount = input;
				break;
			case NoiseType.RiggedMultifractal:
				ridgedModule.OctaveCount = input;
				break;
			case NoiseType.Perlin:
				perlinModule.OctaveCount = input;
				break;
		}
	}

	public void SetSeed(float i) {
		int input = (int )i;

		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.Seed = input;
				break;
			case NoiseType.RiggedMultifractal:
				ridgedModule.Seed = input;
				break;
			case NoiseType.Perlin:
				perlinModule.Seed = input;
				break;
		}
	}

	public void SetScale(float input) {
		scaleBiasModule.Scale = input;
	}

	public void SetBias(float input) {
		scaleBiasModule.Bias = input;
	}
		
}
