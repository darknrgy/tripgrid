using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour {

	public GameObject grid;
	public GameObject spaceCraft;
	public GameObject CubeTemplate;
	public GameObject Explosion;

	private List<GameObject> Cubes = new List<GameObject>();

	void Start () {
		Instantiate(grid);
		Instantiate(spaceCraft);
		CreateCubes();
	}

	public void HitCube(GameObject other){
		UInt32 id = other.GetComponent<Cube>().GetId();
		Debug.Log("HIT ID: " + id);
		GameObject explosion = (GameObject) Instantiate(Explosion);
		explosion.transform.position = other.rigidbody.position;
		explosion.audio.Play ();
		Destroy(explosion, 3.0f);
		Destroy(other);
	}

	void CreateCubes(){
		float gridSize = Config.getF("WorldSize");
		
		for (UInt16 i = 0; i < Config.getI("NumberOfCubes"); i++){
			CreateCube();
		}

	}

	void CreateCube(){
		GameObject cubeChild;
		cubeChild = (GameObject) Instantiate(CubeTemplate);
		//cubeChild.GetComponent<Cube>().SetId((UInt32) Config.GetRandRange(0,100) );
	}

}
