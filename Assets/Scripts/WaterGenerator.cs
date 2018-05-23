using UnityEngine;
using System.Collections;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;

public class WaterGenerator : MonoBehaviour {
    public double frequency;
    public double lacunarity;
    public double persistence;
    public int octaves;
    public float heightMultiplier;

    private int width = 100;
    private int height = 100;

    private TerrainGenerator terrainGenerator;
    private GameObject water;

    private Material material;
    private MeshRenderer waterRenderer;

    void Awake() {
        terrainGenerator = GameObject.FindObjectOfType<TerrainGenerator> ();
    }

	// Use this for initialization
	void Start () {
        GenerateWater();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateWater() {
        // Generate float array for mesh base
        float[,] noiseMap = new float[width, height];

        // If water mesh doesn't exist create it
        if (water == null) {
            water = new GameObject("Water");
            MeshFilter meshFilter = (MeshFilter)water.AddComponent(typeof(MeshFilter));
            meshFilter.mesh = MeshGenerator.createMesh(width / 2, height / 2, 0, noiseMap).createMesh();
            waterRenderer = water.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            material =  new Material(Shader.Find("Custom/WaterShader"));
            waterRenderer.material = material;

            // Scale and position mesh
            water.GetComponent<Renderer> ().transform.localScale = new Vector3 (width/5 , 1, height/5);

            waterRenderer.material.SetFloat("_Value3", terrainGenerator.maxHeight / 6.7f);
        }
        else {  // if water mesh already exist try to position it according to match terrain
            waterRenderer.material.SetFloat("_Value3", terrainGenerator.maxHeight / 6.7f);
        }

    }

    public void SetWaveAmplitude(float input) {
        waterRenderer.material.SetFloat("_Amplitude", input);
    }

    public void SetWaveFrequency(float input) {
        waterRenderer.material.SetFloat("_Frequency", input);
    }

    public void SetFrequency(float input) {
        waterRenderer.material.SetFloat("_Value2", input);
    }

    public void SetScale(float input) {
        waterRenderer.material.SetFloat("_Value1", input);
    }

    public void SetSpeed(float input) {
        waterRenderer.material.SetFloat("_Value4", input);
    }
}
