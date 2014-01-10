using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour {

	public GameObject grid;
	public GameObject spaceCraft;

	// Use this for initialization
	void Start () {
		Instantiate(grid);
		Instantiate(spaceCraft);
	}

	public void HitCube(GameObject other){
		UInt32 id = other.GetComponent<Cube>().GetId();
		Debug.Log("HIT ID: " + id);
	}
}
