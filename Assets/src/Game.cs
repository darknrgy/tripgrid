using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour {

	public GameObject grid;
	public GameObject spaceCraft;
	public GameObject CubeTemplate;
	public GameObject Explosion;

	void Start () {
		Instantiate(grid);
		CreateCubes();
	}

	public void HitCube(GameObject other){
		GameObject explosion = (GameObject) Instantiate(Explosion, other.rigidbody.position, new Quaternion(0,0,0,0));
		explosion.audio.Play ();
		Destroy(explosion, 3.0f);
		Destroy(other);
	}

	void CreateCubes(){
		for (UInt16 i = 0; i < Config.getI("NumberOfCubes"); i++){
			CreateCube();
		}

	}

	void CreateCube(){
		Instantiate(CubeTemplate);
	}

}
