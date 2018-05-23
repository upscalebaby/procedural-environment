using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour {
	public Dropdown menu;
	public TerrainGenerator terrainGenerator;

    public GameObject noise1UI;
    public GameObject noise2UI;
    public GameObject combinedUI;
    public GameObject waterUI;
    public GameObject cloudUI;

	// Use this for initialization
	void Start () {
		ChangeNoiseModule ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeNoiseModule() {
		int i = menu.value;

		switch(i) {
			case 0:
                ActivateNoise1UI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.Billow);
                break;
				
            case 1:
                ActivateNoise2UI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.RidgedMultifractal);
				break;
			case 2:
                ActivateCombinedUI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.Perlin);
                break;
            case 3:
                ActivateWaterUI ();
                break;
            case 4:
                ActivateCloudUI ();
                break;
        }

		terrainGenerator.GenerateTerrain ();

	}
        
	void ActivateNoise1UI() {
		noise1UI.SetActive (true);
        noise2UI.SetActive (false);
        combinedUI.SetActive(false);
        waterUI.SetActive(false);
        cloudUI.SetActive(false);
	}

    void ActivateNoise2UI() {
        noise1UI.SetActive (false);
        noise2UI.SetActive (true);
        combinedUI.SetActive(false);
        waterUI.SetActive(false);
        cloudUI.SetActive(false);
    }

    void ActivateCombinedUI() {
        noise1UI.SetActive (false);
        noise2UI.SetActive (false);
        combinedUI.SetActive(true);
        waterUI.SetActive(false);
        cloudUI.SetActive(false);
    }

    void ActivateWaterUI () {
        noise1UI.SetActive (false);
        noise2UI.SetActive (false);
        combinedUI.SetActive(false);
        waterUI.SetActive(true);
        cloudUI.SetActive(false);
    }

    void ActivateCloudUI () {
        noise1UI.SetActive (false);
        noise2UI.SetActive (false);
        combinedUI.SetActive(false);
        waterUI.SetActive(false);
        cloudUI.SetActive(true);
    }

}
