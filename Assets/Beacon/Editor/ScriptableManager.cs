using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


public class ScriptableManager : MonoBehaviour
{
	[MenuItem("Beacon/Configs/Role")]
	static void CreateRole()
	{
		CreateAsset<RoleConf>();
	}

	[MenuItem("Beacon/Configs/RoleLib")]
	static void CreateRoleLib()
	{
		CreateAsset<RoleLibrary>();
	}

	[MenuItem("Beacon/Configs/Plot")]
	static void CreatePlot()
	{
		CreateAsset<PlotConf>("Assets/Beacon/Configs", true);
	}

	public static void CreateAsset<T>(string newDir = "Assets/Beacon/Configs", bool isSpecify = false) where T : ScriptableObject
	{
		if(isSpecify)
		{
			Create<T>(newDir); 
			return; 
		}
		Create<T>(GetPath()); 
	}

	static void Create<T>(string dir) where T : ScriptableObject
	{
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir); 
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(dir + "/New" + typeof(T).ToString() + ".asset");
		T asset = ScriptableObject.CreateInstance<T>(); 
		AssetDatabase.CreateAsset(asset, assetPathAndName);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	static string GetPath()
	{
		string dir = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (dir == "")
		{
			dir = "Assets/Beacon/Configs";
		}
		else if (string.Compare(Path.GetExtension(dir), "") != 0)
		{
			// if the extension exists, then go to the parent directory
			dir = Path.GetDirectoryName(dir);
		}
		return dir; 
	}
}
