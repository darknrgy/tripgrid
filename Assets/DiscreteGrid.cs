using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiscreteGrid : MonoBehaviour {

	public GameObject Grid;

	Vector3[,,] coreGrid;
	private delegate void Iterate3DCallback(UInt32 ix, UInt32 iy, UInt32 iz);
	private UInt32 lineCounter;
	private GameObject[] lineObjects;


	// Update is called once per frame
	void Update () {
		Iterate3D(new Iterate3DCallback(this.IncreaseX));	
	}

	void UpdateGrid(UInt32 ix, UInt32 iy, UInt32 iz){
		Vector3 start = coreGrid[ix, iy, iz];
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		if (ix < gridCount - 1){
			Vector3 endX  = coreGrid[ix + 1, iy, iz];
			GetLineRenderer(start, endX);
		}
		if (iy < gridCount - 1){
			Vector3 endY  = coreGrid[ix, iy + 1, iz];
			GetLineRenderer(start, endY);
		}
		if (iz < gridCount - 1){
			Vector3 endZ  = coreGrid[ix, iy, iz + 1];		
			GetLineRenderer(start, endZ);
		}
	}

	void CreateGrid(){
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		lineObjects = new GameObject[(int) Math.Pow(gridCount, 4f)];
		coreGrid = new Vector3[gridCount, gridCount, gridCount];
		Iterate3D(new Iterate3DCallback(this.GeneratePoint));
		Iterate3D(new Iterate3DCallback(this.GenerateLine));
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
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		if (ix < gridCount - 1){
			Vector3 endX  = coreGrid[ix + 1, iy, iz];
			GetLineRenderer(start, endX);
		}
		if (iy < gridCount - 1){
			Vector3 endY  = coreGrid[ix, iy + 1, iz];
			GetLineRenderer(start, endY);
		}
		if (iz < gridCount - 1){
			Vector3 endZ  = coreGrid[ix, iy, iz + 1];		
			GetLineRenderer(start, endZ);
		}
	}

	void PrintPoint(UInt32 ix, UInt32 iy, UInt32 iz){
		Debug.Log("point " + ix + iy + iz + coreGrid[ix, iy, iz].ToString());
	}

	LineRenderer GetLineRenderer(Vector3 start, Vector3 end){
		GameObject gameObject = new GameObject();
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, end);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		Color color = new Color(1f, 1f, 1f, 1f);
		lineRenderer.material.SetColor("_TintColor", color);
		lineRenderer.SetWidth(0.08F, 0.08F);
		gameObject.transform.parent = Grid.transform;
		lineRenderer.SetColors(Color.red, Color.blue);
		lineObjects[lineCounter] = gameObject;
		lineCounter ++;
		return lineRenderer;
	}

	void Iterate3D(Iterate3DCallback callback){ Iterate3D(callback, false); }
	void Iterate3D(Iterate3DCallback callback, bool minusOne){
		lineCounter = 0;
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
	



}
