using UnityEngine;
using System.Collections;

public class LogColor
{
	public const string _none = "";
	public const string _red = "<color=red>{0}</color>";
	public const string _blue = "<color=blue>{0}</color>";
	public const string _yellow = "<color=yellow>{0}</color>";
	public const string _black = "<color=black>{0}</color>";
	public const string _purple = "<color=purple>{0}</color>";
	public const string _orange = "<color=orange>{0}</color>";
	public const string _cyan = "<color=cyan>{0}</color>";
	public const string _teal = "<color=teal>{0}</color>";
	public const string _green = "<color=green>{0}</color>";
	public const string _grey = "<color=grey>{0}</color>";
	public const string _magenta = "<color=magenta>{0}</color>";
	public const string _white = "<color=white>{0}</color>";
	public const string _maroon = "<color=maroon>{0}</color>";
}

public class LogUtil 
{
	public static void Log(string s)
	{
		Debug.LogFormat(LogColor._teal, s); 
	}
}
