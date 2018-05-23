using UnityEngine;
using System.Collections;
using LibNoise.Operator;
using LibNoise.Generator;
using LibNoise;

public class TerrainGenerator : MonoBehaviour {

    public enum NoiseType{
        Perlin, Billow, RidgedMultifractal, Combined
    };

	public int width;
	public int height;
	public UnityEngine.Gradient gradient;
    public AnimationCurve curve;
    public float meshScale = 10f;

    public float maxHeight;

	private NoiseGenerator noiseGenerator;
    private GameObject terrain;

	private NoiseType noiseType;
    private bool combineNoise;

	private ModuleBase currentModule;
    private Perlin perlinModule;
    private ScaleBias perlinScaleBiasModule;
    private RidgedMultifractal ridgedModule;
    private ScaleBias ridgedScaleBiasModule;
	private Billow billowModule;
    //private Voronoi billowModule;
    private ScaleBias billowScaleBiasModule;
	private Select selectModule;
    private Turbulence turbulenceModule;
    private ScaleBias turbulenceScaleBias;

    private WaterGenerator waterGenerator;

    private Vector3 offset = new Vector3(-84.28f, 1.42f, 21.42f);
    private float heightMultiplier = 150;
    private float fallOff = -3f;

	// Noise 1 init values
    private float n1frequency = 0.018f;
    private float n1lacunarity = 2.422f;
    private float n1persistence = 0.557f;
	private int n1octaves = 4;
	private int n1seed = 0;
    private float n1scale = 0.113f;
    private float n1bias = -0.412f;

    // Noise 2 init values
    private float n2frequency = 0.025f;
    private float n2lacunarity = 2.114f;
    private int n2octaves = 4;
    private int n2seed = 0;
    private float n2scale = 0.614f;
    private float n2bias = 0.377f;

    // Noise 3 init values
    private float n3frequency = 0.026f;
    private float n3lacunarity = 1.857f;
    private float n3persistence = 0.598f;
    private int n3octaves = 5;
    private int n3seed = 1;
    private float n3scale = 1f;
    private float n3bias = 0.421f;

    void Awake () {
        noiseGenerator = new NoiseGenerator();

        billowModule = new Billow(n1frequency, n1lacunarity, n1persistence, n1octaves, n1seed, QualityMode.High); //(frequency, lacunarity, persistence, octaves, seed, QualityMode.High));
        //billowModule = new Voronoi(n1frequency, n1lacunarity, n1seed, true);
        billowScaleBiasModule = new ScaleBias(n1scale, n1bias, billowModule);

        ridgedModule = new RidgedMultifractal(n2frequency, n2lacunarity, n2octaves, n2seed, QualityMode.High);
        ridgedScaleBiasModule = new ScaleBias(n2scale, n2bias, ridgedModule);

        perlinModule = new Perlin(n3frequency, n3lacunarity, n3persistence, n3octaves, n3seed, QualityMode.High);
        perlinScaleBiasModule = new ScaleBias(n3scale, n3bias, perlinModule);

        selectModule = new Select(billowScaleBiasModule, ridgedScaleBiasModule, perlinScaleBiasModule);
        selectModule.SetBounds(0.5, 1000);  // parameterize?
        selectModule.FallOff = 0.3928571;

        turbulenceModule = new Turbulence(0.335, selectModule);
        turbulenceModule.Frequency = 4.742f;
        turbulenceScaleBias = new ScaleBias(n1scale, n1bias, turbulenceModule);

        currentModule = billowScaleBiasModule;

        waterGenerator = GameObject.FindObjectOfType<WaterGenerator> ();
    }

	// Use this for initialization
	void Start () {
		GenerateTerrain ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateTerrain() {
        
        // Select noise module
        if(!combineNoise) {
            currentModule = turbulenceModule;
        }
        else {
            switch(noiseType) {
                case NoiseType.Perlin:
                    currentModule = perlinScaleBiasModule;
                    break;
                case NoiseType.RidgedMultifractal:
                    currentModule = ridgedScaleBiasModule;
                    break;
                case NoiseType.Billow:
                    currentModule = billowScaleBiasModule;
                    break;
                default:
                    currentModule = perlinScaleBiasModule;
                    break;
            }
        }

		// Generate noiseMap and Falloff map
        float[,] noiseMap = noiseGenerator.GenerateNoise (width, height, offset, currentModule);
        noiseMap = GenerateFallOffMap(noiseMap);

		// Generate mesh
        if (terrain == null) {
            terrain = new GameObject("Terrain");

            MeshFilter meshFilter = (MeshFilter)terrain.AddComponent(typeof(MeshFilter));
            meshFilter.mesh = MeshGenerator.createMesh(width, height, heightMultiplier, noiseMap).createMesh();

            meshFilter.mesh.colors = ColorVertices(meshFilter.mesh);
            MeshRenderer renderer = terrain.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            renderer.material =  new Material(Shader.Find("Custom/TerrainShader"));

            MeshGenerator.MeshData meshData = MeshGenerator.createMesh(width, height, heightMultiplier, noiseMap);

            // Scale mesh
            terrain.GetComponent<Renderer> ().transform.localScale = new Vector3 (width/meshScale , 1, height/meshScale);

            //Find good water level
            maxHeight = meshData.maxHeight;
        } else {
            maxHeight = MeshGenerator.editMesh(width, height, heightMultiplier, noiseMap, terrain.GetComponent<MeshFilter>().mesh);
            terrain.GetComponent<MeshFilter>().mesh.colors = ColorVertices(terrain.GetComponent<MeshFilter>().mesh);
            waterGenerator.GenerateWater();
        }



	}
        
    private Color[] ColorVertices(Mesh mesh) {
        int length = mesh.vertices.Length;
        Color[] colors = new Color[length];
        Vector3[] vertices = mesh.vertices;

        // Find maximum height to make color relative to highest point
        float max = -99999f;
        for (int i = 0; i < length; i++) {
            if (vertices[i].y > max)
                max = vertices[i].y;
        }

        for (int i = 0; i < length; i++) {
            colors[i] = gradient.Evaluate(vertices[i].y / max);
        }

        return colors;

    }

    // Generate Falloff map
    private float[,] GenerateFallOffMap(float[,] heightMap) {
        Vector2 centerOfCircle = new Vector2(width/2, height/2);

        for (int y = 0; y < height - 1; y++) {
            for(int x = 0; x < width - 1; x++) {
                float distanceToCenter = (new Vector2(x, y) - centerOfCircle).magnitude;
                distanceToCenter = Mathf.Max(1, distanceToCenter);
                distanceToCenter = distanceToCenter / (new Vector2(0, 0) - centerOfCircle).magnitude;
                float modifier = (Mathf.Pow(distanceToCenter, fallOff)) / (Mathf.Pow(distanceToCenter, fallOff) + Mathf.Pow((1 - distanceToCenter), fallOff));
                modifier = Mathf.Clamp(modifier, 0, 1);
                heightMap[x, y] = heightMap[x, y] * modifier;
                //heightMap[x, y] = Mathf.Clamp(heightMap[x, y], 0, 1);
            }
        }

        return heightMap;

    }

	// Setters for UI elements
	public void SetNoiseType(NoiseType type) {
		this.noiseType = type;
        GenerateTerrain ();

	}

    public void SetFallOff(float input) {
        this.fallOff = input;
        GenerateTerrain ();
    }

    public void SetX(float input) {
        offset.x = input;
        GenerateTerrain ();
    }

    public void SetY(float input) {
        offset.y = input;
        GenerateTerrain ();
    }

    public void SetZ(float input) {
        offset.z = input;
        GenerateTerrain ();
    }

    public void ToggleCombinedNoise() {
        this.combineNoise = !this.combineNoise;
        GenerateTerrain ();
    }

    public void SetHeightMultiplier(float input) {
        this.heightMultiplier = input;
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
            case NoiseType.Combined:
                turbulenceModule.Frequency = input;
                break;
		}
        GenerateTerrain ();
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
        GenerateTerrain ();
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
        GenerateTerrain ();
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
        GenerateTerrain ();
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
            case NoiseType.Combined:
                turbulenceModule.Seed = input;
                break;
		}
        GenerateTerrain ();
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
            case NoiseType.Combined:
                turbulenceScaleBias.Scale = input;
                break;
        }
        GenerateTerrain ();
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
            case NoiseType.Combined:
                turbulenceScaleBias.Bias = input;
                break;

        }
        GenerateTerrain ();
	}

    public void SetTurbulenceFrequency(float input) {
        turbulenceModule.Frequency = input;
        GenerateTerrain ();
    }

    public void SetTurbulencePower(float input) {
        turbulenceModule.Power = input;
        GenerateTerrain ();
    }

    public void SetTurbulenceRoughness(float i) {
        int input = (int) i;
        turbulenceModule.Roughness = input;
        GenerateTerrain ();
    }
        
    public void SetCrossover(float input) {
        selectModule.FallOff = input;
        GenerateTerrain ();
    }
		
}
