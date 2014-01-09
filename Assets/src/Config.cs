using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Config {

	// defaults
	public Config () {
		prefs.Add ("WorldSize", 40f);
		prefs.Add ("BufferZone", 0f);
		prefs.Add ("GridCount", 20f);
		prefs.Add ("NumberOfCubes", (Int16) 50);
		prefs.Add ("CubeSpeed", 4f);

	}
	
	protected Dictionary<string,object> prefs = new Dictionary<string, object>();
	protected static Config instance = null;
	
	public static object get(string key){return Instance.prefs[key];}
	public static float getF(string key){ return (float) get(key); }
	public static Int16 getI(string key){
		return (Int16) get(key);
	}

	
	public static Config Instance{
		get{
			if (instance == null) instance = new Config();
			return instance;
		}
	}
}
