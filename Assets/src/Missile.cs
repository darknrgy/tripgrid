using UnityEngine;
using System.Collections;
using System;

public delegate void MissileHitEventHandler(object sender, EventArgs e);

public class Missile : MonoBehaviour {


	public GameObject Grid;

	public event MissileHitEventHandler MissileHit;

	protected virtual void OnMissileHit(EventArgs e){
		if (MissileHit != null) MissileHit(this, e);
	}

	void Start () {}
	void Update () {}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Cube")){
			Debug.Log("Cube hit");

			GameObject gameObject = GameObject.FindWithTag("Game");
			Game game = gameObject.GetComponent<Game>();
			game.HitCube(other.gameObject);
		}else{
			Debug.Log("Hit something: " + other.tag);
		}
	}

	public void Test(){
		Debug.Log ("FUCKING SHIT ASS CUNT");
	}
}


