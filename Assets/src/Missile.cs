using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {


	public GameObject Grid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Destroy(this, 3.0f);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Cube")){


			Debug.Log("Cube hit");
		}else{
			Debug.Log("Hit something: " + other.tag);
		}
	}
}
