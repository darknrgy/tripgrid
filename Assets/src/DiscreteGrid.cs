using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Coordinate3D{
	public UInt32 x, y, z;
	public Coordinate3D(UInt32 ix, UInt32 iy, UInt32 iz){
		x = ix;
		y = iy;
		z = iz;
	}
}

public class LineMap{
	public LineRenderer lineRenderer;
	public Coordinate3D start, end;
}

public class DiscreteGrid : MonoBehaviour {

	public GameObject Grid;

	Vector3[,,] coreGrid, dynamicGrid;
	Color[,,] gridColors;
	float[,,] gridLineWidths;
	private delegate void Iterate3DCallback(UInt32 ix, UInt32 iy, UInt32 iz);
	private List<LineMap> lineMapList = new List<LineMap>();

	// Update is called once per frame
	void Update () {
		UpdateShifter();
		Iterate3D(new Iterate3DCallback(this.ShiftLeft));
		//Iterate3D(new Iterate3DCallback(this.AssignColors));
		UpdateLines();
	}

	private float shifter = 0;
	void UpdateShifter(){ shifter += 1 * Time.deltaTime; }

	void ShiftLeft(UInt32 ix, UInt32 iy, UInt32 iz){

		float magnitude = 4f;
		float offset = 1 / (magnitude * 2);

		Vector3 p = coreGrid[ix, iy, iz];
		Vector3 radiusVector = (p - new Vector3(20, 20, 20));
		Vector3 radiusNormalized = radiusVector.normalized;
		float radiusMagnitude = radiusVector.magnitude;
		float sin = (float) Math.Sin(-shifter * 0.2f + radiusMagnitude * 0.15f);
		float deviation = magnitude * sin;

		p = p + radiusNormalized * deviation * (1.0f + radiusMagnitude/50);// * (radiusMagnitude / 10);

		dynamicGrid[ix, iy, iz] = p;

		Vector3 core = coreGrid[ix, iy, iz];
		Vector3 dynam = dynamicGrid[ix, iy, iz];
		
		float diff = deviation;
		// -0.4 to 0.4
		diff = (diff + magnitude) * offset * 2;
		Color color;
		//if (deviation > magnitude * 0.999f) color = new Color(0f, 1f, 0f);
		//else 
		color = new Color(diff - 1f, 0, 1 - (diff - 1f) );
		//color = new Color(1 - (diff - 1f), 0, diff - 1f);
		gridColors[ix, iy, iz] = color;

		float newRadius = (p - new Vector3(20, 20, 20)).magnitude;

		gridLineWidths[ix, iy, iz] = MapRange(newRadius, 0f, 20f, 0.04f, 0.15f);
	}

	// run all the methods to generate coreGrid, dynamicGrid, and lineMapList
	void CreateGrid(){
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		coreGrid = new Vector3[gridCount, gridCount, gridCount];
		gridColors = new Color[gridCount, gridCount, gridCount];
		gridLineWidths = new float[gridCount, gridCount, gridCount];
		dynamicGrid = new Vector3[gridCount, gridCount, gridCount];
		Iterate3D(new Iterate3DCallback(this.GeneratePoint));
		Array.Copy(coreGrid, dynamicGrid, coreGrid.Length);
		Iterate3D(new Iterate3DCallback(this.AssignColors));
		Iterate3D(new Iterate3DCallback(this.GenerateLine));

	}

	// calculate world positions from coordinates, assign to coreGrid
	void GeneratePoint(UInt32 ix, UInt32 iy, UInt32 iz){
		float x, y, z;
		float segment = Config.getF("WorldSize") / (UInt32) Config.getI("GridCount");
		x = ix * segment;
		y = iy * segment;
		z = iz * segment;
		Vector3 position = new Vector3(x, y, z);
		coreGrid[ix, iy, iz] = position;
	}

	// assign colors based on position
	void AssignColors(UInt32 ix, UInt32 iy, UInt32 iz){
		Vector3 core = coreGrid[ix, iy, iz];
		Vector3 dynam = dynamicGrid[ix, iy, iz];

		float diff = dynam.y - core.y;
		// -0.4 to 0.4
		diff = (diff + 0.4f) * 1.25f;
		Color color = new Color(diff, 0, 1f - diff);
		gridColors[ix, iy, iz] = color;
		gridLineWidths[ix, iy, iz] = 0.08f;
	}


	// create the original coreGrid 
	void GenerateLine(UInt32 ix, UInt32 iy, UInt32 iz){
		Coordinate3D start = new Coordinate3D(ix, iy, iz);
		UInt32 gridCount = (UInt32) Config.getI("GridCount");
		if (ix < gridCount - 1){
			Coordinate3D endX = new Coordinate3D(ix + 1, iy, iz);
			GetLineRenderer(ref start, ref endX);
		}
		if (iy < gridCount - 1){
			Coordinate3D endY = new Coordinate3D(ix, iy + 1, iz);
			GetLineRenderer(ref start, ref endY);
		}
		if (iz < gridCount - 1){
			Coordinate3D endZ = new Coordinate3D(ix, iy, iz + 1);
			GetLineRenderer(ref start, ref endZ);
		}
	}

	// copy from dynamicGrid to the actual lineRenderers
	void UpdateLines(){
		foreach (LineMap lineMap in lineMapList){
			LineRenderer lineRenderer = lineMap.lineRenderer;
			lineRenderer.SetPosition(0, dynamicGrid[lineMap.start.x, lineMap.start.y, lineMap.start.z]);
			lineRenderer.SetPosition(1, dynamicGrid[lineMap.end.x, lineMap.end.y, lineMap.end.z]);
			lineRenderer.SetColors(gridColors[lineMap.start.x, lineMap.start.y, lineMap.start.z], gridColors[lineMap.end.x, lineMap.end.y, lineMap.end.z]);
			lineRenderer.SetWidth(gridLineWidths[lineMap.start.x, lineMap.start.y, lineMap.start.z], gridLineWidths[lineMap.end.x, lineMap.end.y, lineMap.end.z]);
		}

	}

	// print a point
	void PrintPoint(UInt32 ix, UInt32 iy, UInt32 iz){
		//Debug.Log("point " + ix + iy + iz + dynamicGrid[ix, iy, iz].ToString());
	}

	// create LineRenderer and associated GameObject
	LineRenderer GetLineRenderer(ref Coordinate3D start, ref Coordinate3D end){
		GameObject gameObject = new GameObject();
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetPosition(0, dynamicGrid[start.x, start.y, start.z]);
		lineRenderer.SetPosition(1, dynamicGrid[end.x, end.y, end.z]);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		Color color = new Color(1f, 1f, 1f, 1f);
		lineRenderer.material.SetColor("_TintColor", color);
		lineRenderer.SetWidth(gridLineWidths[start.x, start.y, start.z], gridLineWidths[end.x, end.y, end.z]);
		gameObject.transform.parent = Grid.transform;
		lineRenderer.SetColors(gridColors[start.x, start.y, start.z], gridColors[end.x, end.y, end.z]);
		LineMap lineMap = new LineMap();
		lineMap.start = start;
		lineMap.end = end;
		lineMap.lineRenderer = lineRenderer;
		lineMapList.Add(lineMap);
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

	private Vector3 a = new Vector3(0, 0, 0);
	private Vector3 b;


	// Use this for initialization
	void Start () {

		/*for (float a = -1f; a <= 1f; a += 0.1f){
			Debug.Log(MapRange(a, -1f, 1f, -5f, 0f));
		}*/

		CreateGrid();

	}

	float MapRange(float input, float inLow, float inHigh, float outLow, float outHigh){
		return  outLow + (((input - inLow) / (inHigh - inLow)) * (outHigh - outLow));
	}
	



}
