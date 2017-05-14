using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI; 
using UnityEditor;
using System.IO;

 
public class Test : MonoBehaviour
{
	void Start()
	{
		
		Debug.Log("obj: " + Selection.activeObject); 
		Debug.Log("active path: " + AssetDatabase.GetAssetPath(Selection.activeObject)); 
		Debug.Log("active filename: " + Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject))); 
	}
}
