using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;


public class Config {

	// defaults
	public Config () {
		prefs.Add ("WorldSize", 40f);
		prefs.Add ("BufferZone", 15f);
		prefs.Add ("GridCount", 20f);
		prefs.Add ("NumberOfCubes", (Int16) 15);
		prefs.Add ("CubeSpeed", 4f);
		rand = new Random();

	}

	protected Dictionary<string,object> prefs = new Dictionary<string, object>();
	protected static Config instance = null;
	public static object get(string key){return Instance.prefs[key];}
	public static float getF(string key){ return (float) get(key); }
	public static Int16 getI(string key){ return (Int16) get(key); }


	private Random rand;

	
	public static Config Instance{
		get{
			if (instance == null) instance = new Config();
			return instance;
		}
	}

	public static Random GetRand(){ 
		return Instance.rand;
	}

	public static float GetRandRange(float from, float to){
		return (float) Instance.rand.NextDouble() * (to - from) + from;
	}


	public static Vector3[] GetDirections(){ 
		return new Vector3[6]{
			Vector3.up,
			Vector3.down,
			Vector3.left,
			Vector3.right,
			Vector3.forward,
			Vector3.back
		};
	}
}
