using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour {
	public GameObject perlinUI;
	public GameObject ridgedUI;
	public GameObject billowUI;
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
				ActivatePerlinUI ();
				terrainGenerator.SetNoiseType (NoiseType.Perlin);
				break;
			case 1:
				ActivateRidgeUI ();
				terrainGenerator.SetNoiseType (NoiseType.RiggedMultifractal);
				break;
			case 2:
				ActivateBillowUI ();
				terrainGenerator.SetNoiseType (NoiseType.Billow);
				break;
			}

		//terrainGenerator.GenerateTerrain ();
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

	void ActivateBillowUI() {
		perlinUI.SetActive (false);
		ridgedUI.SetActive (false);
		billowUI.SetActive (true);
	}



}
