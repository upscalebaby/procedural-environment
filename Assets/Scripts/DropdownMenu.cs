using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour {
	public GameObject perlinUI;
	public GameObject ridgedUI;
	public GameObject billowUI;
    public GameObject combinedUI;
	public Dropdown menu;
	public TerrainGenerator terrainGenerator;

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
                ActivateBillowUI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.Billow);
                break;
				
            case 1:
                ActivateRidgeUI ();
                terrainGenerator.SetNoiseType(TerrainGenerator.NoiseType.RidgedMultifractal);
				break;
			case 2:
                ActivatePerlinUI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.Perlin);
                break;
            case 3:
                ActivateCombinedUI ();
                terrainGenerator.SetNoiseType (TerrainGenerator.NoiseType.Combined);
                break;
			}

		terrainGenerator.GenerateTerrain ();
	}



	void ActivatePerlinUI() {
		perlinUI.SetActive (true);
		ridgedUI.SetActive (false);
		billowUI.SetActive (false);
        combinedUI.SetActive(false);
	}

	void ActivateRidgeUI() {
		perlinUI.SetActive (false);
		ridgedUI.SetActive (true);
		billowUI.SetActive (false);
        combinedUI.SetActive(false);
	}

	void ActivateBillowUI() {
		perlinUI.SetActive (false);
		ridgedUI.SetActive (false);
		billowUI.SetActive (true);
        combinedUI.SetActive(false);
	}

    void ActivateCombinedUI () {
        perlinUI.SetActive (false);
        ridgedUI.SetActive (false);
        billowUI.SetActive (false);
        combinedUI.SetActive (true);
    }



}
