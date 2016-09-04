using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour {
	GameObject perlinUI;
	GameObject ridgedUI;
	GameObject billowUI;
	Dropdown menu;

	TerrainGenerator terrainGenerator;

	// Use this for initialization
	void Start () {
		perlinUI = GameObject.FindGameObjectWithTag ("perlin");
		ridgedUI = GameObject.FindGameObjectWithTag ("ridged");
		billowUI = GameObject.FindGameObjectWithTag ("billow");
		menu = GameObject.FindObjectOfType<UnityEngine.UI.Dropdown> ();
		terrainGenerator = GameObject.FindObjectOfType<TerrainGenerator> ();

		ChangeNoiseModule ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeNoiseModule() {
		int i = menu.value;

		switch(i) {
			case 0:
				ActivatePerlinUI ();
				terrainGenerator.SetNoiseType (NoiseType.Perlin);
				break;
			case 1:
				ActivateRidgeUI ();
				terrainGenerator.SetNoiseType (NoiseType.RiggedMultifractal);
				break;
			case 2:
				ActivatebillowUI ();
				terrainGenerator.SetNoiseType (NoiseType.Billow);
				break;
			}

		terrainGenerator.GenerateTerrain ();
	}

	void ActivatePerlinUI() {
		perlinUI.SetActive (true);
		ridgedUI.SetActive (false);
		billowUI.SetActive (false);

	}

	void ActivateRidgeUI() {
		perlinUI.SetActive (false);
		ridgedUI.SetActive (true);
		billowUI.SetActive (false);
	}

	void ActivatebillowUI() {
		perlinUI.SetActive (false);
		ridgedUI.SetActive (false);
		billowUI.SetActive (true);
	}



}
