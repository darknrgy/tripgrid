using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiscreteGrid : MonoBehaviour {

	public GameObject Grid;

	Vector3[,,] coreGrid;
	private delegate void Iterate3DCallback(UInt32 ix, UInt32 iy, UInt32 iz);

	void CreateGrid(){
		Debug.Log("create grid?");
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		coreGrid = new Vector3[gridCount, gridCount, gridCount];
		Iterate3D(new Iterate3DCallback(this.GeneratePoint));
		Iterate3D(new Iterate3DCallback(this.GenerateLine), true);
	}

	void GeneratePoint(UInt32 ix, UInt32 iy, UInt32 iz){
		float x, y, z;
		float segment = Config.getF("WorldSize") / (UInt32) Config.getI("GridCount");
		x = ix * segment;
		y = iy * segment;
		z = iz * segment;
		Vector3 position = new Vector3(x, y, z);
		coreGrid[ix, iy, iz] = position;
	}

	void GenerateLine(UInt32 ix, UInt32 iy, UInt32 iz){
		Vector3 start = coreGrid[ix, iy, iz];
		Vector3 endX  = coreGrid[ix + 1, iy, iz];
		Vector3 endY  = coreGrid[ix, iy + 1, iz];
		Vector3 endZ  = coreGrid[ix, iy, iz + 1];
		GetLineRenderer(start, endX);
		GetLineRenderer(start, endY);
		GetLineRenderer(start, endZ);

	}

	LineRenderer GetLineRenderer(Vector3 start, Vector3 end){
		Debug.Log("creating line rendererrerer");
		GameObject gameObject = new GameObject();
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(0, end);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		Color color = new Color(1f, 1f, 1f, 1f);
		lineRenderer.material.SetColor("_TintColor", color);
		lineRenderer.SetWidth(0.08F, 0.08F);
		gameObject.transform.parent = Grid.transform;
		lineRenderer.SetColors(Color.white, Color.white);
		return lineRenderer;
	}

	void Iterate3D(Iterate3DCallback callback){ Iterate3D(callback, false); }
	void Iterate3D(Iterate3DCallback callback, bool minusOne){
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		if (minusOne) gridCount -= 1;
		for (UInt32 ix = 0; ix < gridCount; ix ++){
			for (UInt32 iy = 0; iy < gridCount; iy ++){
				for (UInt32 iz = 0; iz < gridCount; iz ++){
					callback(ix, iy, iz);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		CreateGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
