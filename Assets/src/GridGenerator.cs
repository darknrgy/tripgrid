using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = System.Random;

public class GridGenerator : MonoBehaviour {

	public Material trailMaterial;
	public GameObject Grid;
	public GameObject GlobalConfig;

	void Start () {
		CreateGrid();
	}

	void CreateGrid(){
		LineRenderer line;
		Color c1 = new Color();
		Color c2 = new Color();

		float gridSize = Config.getF("WorldSize");
		float gridCount = Config.getF("GridCount");
		
		for (float u = 0; u <= gridSize; u += gridSize / gridCount){
			for (float v = 0; v <= gridSize; v += gridSize / gridCount){
				line = GetLineRenderer();
				line.SetPosition(0, new Vector3(u, v, 0));
				line.SetPosition(1, new Vector3(u, v, gridSize));
				
				c1 = Color.red;
				c2 = Color.blue;
				line.SetColors(c1, c2);
				
				line = GetLineRenderer();
				line.SetPosition(0, new Vector3(0, u, v));
				line.SetPosition(1, new Vector3(gridSize, u, v));
				
				c1 = Color.green;
				c2 = Color.blue;
				line.SetColors(c1, c2);

				line = GetLineRenderer();
				line.SetPosition(0, new Vector3(u, 0, v));
				line.SetPosition(1, new Vector3(u, gridSize, v));
				
				c1 = Color.blue;
				c2 = Color.red;
				line.SetColors(c1, c2);
			}
		}
	}

	void SaveGrid(GameObject Grid){
		//PrefabUtility.CreatePrefab("Assets/PrefabGrid.prefab", Grid, ReplacePrefabOptions.ConnectToPrefab);
	}

	protected LineRenderer GetLineRenderer(){
		GameObject gameObject = new GameObject();		
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		//Color color = lineRenderer.material.color;
		//color.a = 0.0f;
		//lineRenderer.material.color = color;
		lineRenderer.SetWidth(0.08F, 0.08F);
		gameObject.transform.parent = Grid.transform;
		return lineRenderer;
	}

	protected LineRenderer GetLineFromObject(GameObject lineObject){
		return lineObject.GetComponent<LineRenderer>();
	}


	Vector3 MixVectors(Vector3 a, Vector3 b){
		a.x *= b.x;
		a.y *= b.y;
		a.z *= b.z;
		return a;
	}
}
