using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyApplication : MonoBehaviour {

	public GameObject gameState;
	public GameObject grid;
	public GameObject spaceCraft;
	public GameObject missileTemplate;

	private GameObject state = null;

	// Use this for initialization
	void Start () {

		SwitchState(gameState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SwitchState(GameObject newState){
		if (state != null) Destroy(state);
		state = (GameObject) Instantiate(newState);
	}
}
