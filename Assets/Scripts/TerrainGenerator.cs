﻿using UnityEngine;
using System.Collections;
using LibNoise.Operator;
using LibNoise.Generator;
using LibNoise;

public class TerrainGenerator : MonoBehaviour {

    public enum NoiseType{
        Perlin, Billow, RidgedMultifractal
    };

	public int width;
	public int height;
	public UnityEngine.Gradient gradient;

	private NoiseGenerator noiseGenerator;
	private NoiseType noiseType;

	private ModuleBase currentModule;

    private Perlin perlinModule;
    private ScaleBias perlinScaleBiasModule;

    private RidgedMultifractal ridgedModule;
    private ScaleBias ridgedScaleBiasModule;

	private Billow billowModule;
	private ScaleBias billowScaleBiasModule;

	private Select selectModule;
    private bool combinedNoise;

	private GameObject terrainMesh;

	// Init values for modules
	private float frequency = 0.06f;
	private float lacunarity = 2f;
	private float persistence = 0.5f;
	private int octaves = 4;
	private int seed = 0;

	private float scale = 0.15f;
	private float bias = -0.22f;

    void Awake () {
        noiseGenerator = new NoiseGenerator();
        perlinModule = new Perlin(frequency, lacunarity, persistence, octaves, seed, QualityMode.High);
        perlinScaleBiasModule = new ScaleBias(scale, bias, perlinModule);

        ridgedModule = new RidgedMultifractal(frequency, lacunarity, octaves, seed, QualityMode.High);
        ridgedScaleBiasModule = new ScaleBias(scale, bias, ridgedModule);

        billowModule = new Billow(frequency, lacunarity, persistence, octaves, seed, QualityMode.High);
        billowScaleBiasModule = new ScaleBias(scale, bias, billowModule);

        selectModule = new Select(billowScaleBiasModule, ridgedScaleBiasModule, perlinScaleBiasModule);
        selectModule.SetBounds(0.7f, 1000);
        selectModule.FallOff = 0.125;
    }

	// Use this for initialization
	void Start () {
		GenerateTerrain ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateTerrain() {
		ModuleBase noiseModule;

        // Select the right noise module
        if(combinedNoise) {
            noiseModule = selectModule;
        }
        else {
            switch(noiseType) {
                case NoiseType.Perlin:
                    noiseModule = perlinScaleBiasModule;
                    break;
                case NoiseType.RidgedMultifractal:
                    noiseModule = ridgedScaleBiasModule;
                    break;
                case NoiseType.Billow:
                    noiseModule = billowScaleBiasModule;
                    break;
                default:
                    noiseModule = perlinScaleBiasModule;
                    break;
            }
        }

		// Generate noiseMap
		float[,] noiseMap = noiseGenerator.GenerateNoise (width, height, noiseModule);

		// Generate texture
		Texture2D texture = GenerateTexture (noiseMap);

		// Generate mesh
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

    public void ToggleCombinedNoise() {
        this.combinedNoise = !this.combinedNoise;
        GenerateTerrain ();
    }
        
	public void SetFrequency(float input) {
		switch(noiseType) {
			case NoiseType.Billow:
				billowModule.Frequency = input;
				break;
			case NoiseType.RidgedMultifractal:
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
			case NoiseType.RidgedMultifractal:
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
			case NoiseType.RidgedMultifractal:
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
			case NoiseType.RidgedMultifractal:
				ridgedModule.Seed = input;
				break;
			case NoiseType.Perlin:
				perlinModule.Seed = input;
				break;
		}
	}

	public void SetScale(float input) {
        switch(noiseType) {
            case NoiseType.Billow:
                billowScaleBiasModule.Scale = input;
                break;
            case NoiseType.RidgedMultifractal:
                ridgedScaleBiasModule.Scale = input;
                break;
            case NoiseType.Perlin:
                perlinScaleBiasModule.Scale = input;
                break;
        }
	}

	public void SetBias(float input) {
        switch(noiseType) {
            case NoiseType.Billow:
                billowScaleBiasModule.Bias = input;
                break;
            case NoiseType.RidgedMultifractal:
                ridgedScaleBiasModule.Bias = input;
                break;
            case NoiseType.Perlin:
                perlinScaleBiasModule.Bias = input;
                break;
        }
	}
		
}
