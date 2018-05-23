using UnityEngine;
using System.Collections;

public class CloudGenerator : MonoBehaviour {
    public float height = 20;
    private GameObject clouds;
    private MeshRenderer cloudRenderer;
    private Material material;

	// Use this for initialization
	void Start () {
        GenerateClouds();
	}

    public void GenerateClouds() {
        float[,] noiseMap = new float[100, 100];

        // create mesh and add shader to it
        clouds = new GameObject("Clouds");
        MeshFilter meshFilter = (MeshFilter)clouds.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = MeshGenerator.createMesh(100 / 2, 100 / 2, 0, noiseMap).createMesh();
        cloudRenderer = clouds.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        material =  new Material(Shader.Find("Custom/CloudShader"));
        cloudRenderer.material = material;

        // Scale and position mesh
        clouds.GetComponent<Renderer> ().transform.localScale = new Vector3 (100/5 , 1, 100/5);

        clouds.transform.Translate(Vector3.up * height);

    }

    public void SetWaveAmplitude(float input) {
        cloudRenderer.material.SetFloat("_WaveAmplitude", input);
    }

    public void SetWaveFrequency(float input) {
        cloudRenderer.material.SetFloat("_WaveFrequency", input);
    }

    public void SetFrequency(float input) {
        cloudRenderer.material.SetFloat("_Frequency", input);
    }

    public void SetScale(float input) {
        cloudRenderer.material.SetFloat("_Scale", input);
    }

    public void SetSpeed(float input) {
        cloudRenderer.material.SetFloat("_Speed", input);
    }

    public void SetBias(float input) {
        cloudRenderer.material.SetFloat("_Bias", input);
    }
}
