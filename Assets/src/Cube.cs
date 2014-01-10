using UnityEngine;
using System.Collections;
using System;

public class Cube : MonoBehaviour {

	private UInt32 id;
	public void SetId(UInt32 setId){ id = setId; }
	public UInt32 GetId(){ return id; }

	void Start(){
		float gridSize = Config.getF("WorldSize");
		rigidbody.position = new Vector3(
			Config.GetRandRange(0f, gridSize), 
			Config.GetRandRange(0f, gridSize), 
			Config.GetRandRange(0f, gridSize));
		audio.pitch = Config.GetRandRange(0.3f, 1.0f);

		SetRandomVelocity();
	}


	void FixedUpdate(){

		float worldSize = Config.getF("WorldSize");
		float bufferZone = Config.getF("BufferZone");
		
		float min = 0 - bufferZone;
		float max = worldSize + bufferZone;
			

		Vector3 position = rigidbody.position;
		Vector3 velocity = rigidbody.velocity;
		
		if (position.x < min) position.x = max;
		if (position.x > max) position.x = min;
		
		if (position.y < min) position.y = max;
		if (position.y > max) position.y = min;
		
		
		if (position.z < min) position.z = max;
		if (position.z > max) position.z = min;
		rigidbody.position = position;
		
		if (Config.GetRandRange(0, 1000) > 999.9){
			SetRandomVelocity();
		}
		
		rigidbody.velocity = velocity;
	
	}

	public void SetRandomVelocity(){
		Vector3[] directions = Config.GetDirections();
		rigidbody.velocity = directions[Config.GetRand().Next(directions.Length)] * Config.getF("CubeSpeed");
	}
}
