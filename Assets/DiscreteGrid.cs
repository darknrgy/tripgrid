using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiscreteGrid : MonoBehaviour {

	public GameObject Grid;

	Vector3[,,] coreGrid;
	private delegate void Iterate3DCallback(UInt32 ix, UInt32 iy, UInt32 iz);

	void CreateGrid(){
		UInt32 segments = (UInt32) Config.getI("GridCount");
		coreGrid = new Vector3[segments, segments, segments];
		Iterate3D(new Iterate3DCallback(this.GeneratePoints));
	}

	void Iterate3D(Iterate3DCallback callback){
		UInt32 segments = (UInt32) Config.getI("GridCount");

		for (UInt32 ix = 0; ix <= segments; ix ++){
			for (UInt32 iy = 0; iy <= segments; iy ++){
				for (UInt32 iz = 0; iz <= segments; iz ++){
					callback(ix, iy, iz);
				}
			}
		}
	}

	void GeneratePoints(UInt32 ix, UInt32 iy, UInt32 iz){
		float x, y, z;
		float segment = Config.getF("WorldSize") / (UInt32) Config.getI("GridCount");
		x = ix * segment;
		y = iy * segment;
		z = iz * segment;
		Vector3 position = new Vector3(x, y, z);
		coreGrid[ix, iy, iz] = position;
	}



	// Use this for initialization
	void Start () {
		CreateGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
