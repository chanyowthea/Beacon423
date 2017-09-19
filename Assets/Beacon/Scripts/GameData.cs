using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System; 


public static class DirCode
{
	public const byte EAST = 1; 
	public const byte WEST = 2; 
	public const byte SOUTH = 4; 
	public const byte NORTH = 8; 
}

public static class MapCode
{
	public const char NOT_EXIST = 'z'; 
	public const char NONE = '0'; 
	public const char WALL = '1'; 
	public const char PIT = '2'; 
	public const char ENEMY = '3'; 
	public const char NPC = '4'; 
	public const char NPC_DARK_PRINCE = '5'; 
	public const char PLAYER = '7'; 
	public const char BEFORE_DOWNSTAIR = '8'; 
	public const char BEFORE_UPSTAIR = '9'; 
}

public struct Pos
{
	public int _x; 
	public int _y; 
	public Pos(int x, int y)
	{
		_x = x; 
		_y = y; 
	}

	public static bool IsIdentical(Pos pos1, Pos pos2)
	{
		if (pos1._x == pos2._x && pos1._y == pos2._y)
		{
			return true; 
		}
		return false; 
	}
}

public class GameData : MonoBehaviour
{
	public static GameData _Instance; 

	#region Main

	void Awake()
	{
		_Instance = this; 
	}

	public void Init()
	{
		InitPlayer(); 
	}

	public void Reset()
	{
		ResetPlayer(); 
		ResetGame(); 
		ResetUI(); 
	}

	public void Clear()
	{
		
	}
	#endregion





	// MapManager
	#region Map
	[SerializeField] public Transform _wallPrefab;
	[NonSerialized] public Transform _wallParent; 
	#endregion

	#region Items

	[SerializeField] public Transform _pitPrefab; 
	#endregion

	#region Player

	[SerializeField] public Transform _playerPrefab; 
	[SerializeField] public Transform _enemyPrefab;
	[SerializeField] public Transform _npcPrefab; 
	[SerializeField] public Transform _hitPointPrefab; 
	#endregion

	#region Plot
	[SerializeField] public PlotConf _plot_Meet;  
	[SerializeField] public PlotConf _plot_Battle;
	[SerializeField] public PlotConf _plot_Start; 
	[SerializeField] public RoleLibrary _roleLib; 
	[SerializeField] public float _conversSpeed = 1f; 
	public static int _curMeetHint = -1; 
	#endregion



	// GameManager
	#region Level

	public static int _CurLevel = 1;
	public const int _MaxLevel = 4;



	#endregion
	void InitGame()
	{
		ResetGame(); 
	}

	void ResetGame()
	{
		_CurLevel = 1; 
	}

	// UIManager
	#region Tutorial

	public static bool _IsOpenTutorial = true;
	public static bool _IsShowedOpenDoorTutorial = true;

	#endregion

	#region TipsCount

	#endregion



	void InitUI()
	{
		
	}

	void ResetUI()
	{
		
	}

	#region Step

	public static int _Step = 0;
	#endregion


	#region Player
	public static bool _CanRotateCamera = false;
	public static bool _HasRotated = false;
	public static bool _isOpenMask = true; 
	void InitPlayer()
	{
		ResetPlayer(); 
	}

	void ResetPlayer()
	{
		_CanRotateCamera = false; 
		_HasRotated = false; 
		_Step = 0; 
	}
	#endregion
}
