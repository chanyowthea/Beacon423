using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum EDirection
{
	East, 
	West, 
	South, 
	North
}

public class Player : MonoBehaviour
{
	#region Main

	public static Player _Instance; 	

	[SerializeField] int _x;
	[SerializeField] int _y;

	void Awake()
	{
		_Instance = this; 
	}

	public void Init()
	{
		Debug.Log("isUpstairs: " + _isUpstairs); 
		InitLevel(); 
		Reset(); 
		InitHP(); 
	}

	public void Reset()
	{
		MapManager.DestroyWall(); 
		UIManager._Instance.Reset(); 
		UIManager._Instance.SetStep(GameData._Step); 
		MapManager._curMap = MapManager.LoadMap(); 
		MapManager.GenerateWall(MapManager._curMap, MapManager._width, MapManager._height); 
		ResetPlayerPos(); 
		UIManager._Instance.SetCurLevel(GameData._CurLevel); 
		ResetHP(); 
		SetRoleInfo(ERole.Grandpa); 
	}

	public void Clear()
	{
		ClearRoleInfo(); 
		ClearMeet(); 
	}

	#endregion


	#region Role Info
//	[SerializeField] TextMesh _nameText; 

	void SetRoleInfo(ERole roleIdent)
	{
//		RoleConf role = GameData._Instance._roleLib.GetRole(roleIdent); 
//		if (role != null)
//		{ 
//			_nameText.text = role._name; 
//		}
	}

	void ClearRoleInfo()
	{
//		_nameText.text = null; 
	}
	#endregion


	#region Move

	const byte _DIR_EAST = 1;
	const byte _DIR_WEST = 2;
	const byte _DIR_SOUTH = 4;
	const byte _DIR_NORTH = 8;
	bool isLockMove; 

	void ResetPlayerPos()
	{
		char role = _isUpstairs ? '8' : '9'; // 上楼后达到的位置
		for (int i = 0; i < MapManager._curMap.Length; ++i)
		{
			if (MapManager._curMap[i] == role)
			{
				_x = i % MapManager._width - MapManager._width / 2; 
				_y = i / MapManager._width - MapManager._height / 2; 
				break; 
			}
		}

		Vector2[] dirs = new Vector2[4]; 
		dirs[0] = new Vector2(1, 0); 
		dirs[1] = new Vector2(-1, 0); 
		dirs[2] = new Vector2(0, -1); 
		dirs[3] = new Vector2(0, 1); 
		float[] angles = new float[]{-90, 90, 180, 0}; 
		transform.eulerAngles = Vector3.zero; 
		for (int i = 0, count = dirs.Length; i < count; ++i)
		{
			int index = MapManager.CurIndex(_x + (int)dirs[i].x, _y + (int)dirs[i].y); 
			if (index > 0) 
			{
				char c = 
				MapManager._curMap[index]; 
				if (c == '0')
				{
					Vector3 angle = new Vector3(0, 0, angles[i]); 
					transform.localEulerAngles += angle; 
					UIManager._Instance.ChangeCompassDir(-angle); 
					GameData._HasRotated = i <= 1;
//					Debug.Log("HasRotated: " + GameData._HasRotated); 
					break; 
				}
			}
		}
	}

	void Update()
	{
		if (isLockMove)
		{
			return; 
		}
		float h = Input.GetAxisRaw("Horizontal"); 
		float v = Input.GetAxisRaw("Vertical"); 
		if (Input.GetButtonDown("Horizontal"))
		{
			if (GameData._CanRotateCamera)
			{
				Vector3 angle = new Vector3(0, 0, (h < 0 ? 1 : -1) * 90); 
//                Camera.main.transform.eulerAngles += angle; 
				Player._Instance.transform.eulerAngles += angle; 
				UIManager._Instance.ChangeCompassDir(-angle); 
				GameData._HasRotated = !(GameData._HasRotated); 
			}
			else
			{
				Move((int)h, (int)v); 
			}
		}
		else if (Input.GetButtonDown("Vertical"))
		{
//			Debug.Log("can rotate: " + GameData._CanRotateCamera + ", is rotate: " + GameData._HasRotated); 
			if (GameData._CanRotateCamera)
			{
				int rotateTimes = (int)(Camera.main.transform.eulerAngles.z / 90); 
				int changeValue = (rotateTimes == 1 || rotateTimes == 2) ? -(int)v : (int)v; 
				if (GameData._HasRotated)
				{
					Move(changeValue, 0); 
				}
				else
				{
					Move(0, changeValue); 
				}
			}
			else
			{ 
				Move((int)h, (int)v); 
			}
		}
		transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0); 
	}

	void Move(int x, int y) // 3 Enemy, 2 Pit, 1 Wall, 0 Road, 
	{
		int newX = _x + (int)x; 
		int newY = _y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

		if (curIndex >= 0)
		{
//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 
			if (MapManager._curMap[curIndex] == '0' || MapManager._curMap[curIndex] == '9' || MapManager._curMap[curIndex] == '8')
			{
				if (GameData._curMeetHint <= 0)
				{
					int idx = MapManager.CurIndex(newX + 1, newY + 1); 
					if (idx >= 0 && MapManager._curMap[idx] == MapCode.NPC)
					{
						GameObject go = MapManager.GetObj(idx); 
						NPC npc = null; 
						if (go != null)
						{
							npc = go.GetComponent<NPC>();
						}
						if (npc != null && npc._roleIdent == ERole.GrandDaughter)
						{
							if (GameData._curMeetHint == 0)
							{
								UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Find); 
							}
							else
							{
								UIManager._Instance.SetSysMsgInfo(SystemMessage._meet_Find); 
							}
							GameData._curMeetHint = 1; 

							// start plot
							isLockMove = true; 
							_onMeetFinish = () =>
								{
									isLockMove = false; 
								}; 
							_meetRoutine = MeetRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo); 
							CoroutineUtil.Start(_meetRoutine); 
						}
					}
				}
				int index = MapManager.CurIndex(_x, _y); 
				_x = newX; 
				_y = newY; 
				ChangeStep(GameData._Step + 1);
				CalculateDirection();
			}
			else
			{
				CalculateDirection(false); // 切换下一关不要显示tip
			}

			// 下面都是未移动
			if (MapManager._curMap[curIndex] == MapCode.ENEMY)
			{
				// do hurt
				GameObject go = MapManager.GetObj(curIndex); 
				if (go != null)
				{
					Enemy enemy = go.GetComponent<Enemy>(); 
					int count = 1; 
					bool rs = enemy.MinusHP(count); 
					if (rs)
					{
						UIManager._Instance.SetSysMsgInfo(string.Format("给敌人{0}点伤害！", count)); // TODO 要设置敌人的攻击力，玩家的防御力
					}
				}
			}
			else if (MapManager._curMap[curIndex] == MapCode.PIT)
			{
				int count = 1; 
				MinusHP(count); 
				UIManager._Instance.SetSysMsgInfo(string.Format("你受到{0}点伤害！", count)); 
			}
			else if (MapManager._curMap[curIndex] == MapCode.NPC)
			{
//				GameObject go = MapManager.GetObj(curIndex); 
			}
		}
		else
		{
			CalculateDirection(); 
			NextLevel(GameData._CurLevel + (MapManager._curMap[MapManager.CurIndex(_x, _y)] == '9' ? 1 : -1)); 
		}
	}

	#endregion



	#region Common

	public Pos GetPos()
	{
		return new Pos(_x, _y); 
	}

	int CheckPit(ref byte dir, byte constDir, Vector2 pos)
	{
		int curIndex = MapManager.CurIndex((int)pos.x, (int)pos.y); 
		if (MapManager._curMap[curIndex] == '2')
		{
			dir |= constDir; 
			return 1; 
		}
		return -1; 
	}


	int CheckEnemy(ref byte dir, byte constDir, Vector2 pos)
	{
		int curIndex = MapManager.CurIndex((int)pos.x, (int)pos.y); 
		if (MapManager._curMap[curIndex] == '3')
		{
			dir |= constDir; 
			return 1; 
		}
		return -1; 
	}
	#endregion



	#region Data
	void ChangeStep(int step)
	{
		GameData._Step = step; // 新的x，y必须是可以行走的才加步数 
		UIManager._Instance.SetStep(GameData._Step); 
	}
	#endregion



	#region Tips
	void CalculateDirection(bool isMoved = true)
	{
		byte[] constDir = new byte[4]{ _DIR_EAST, _DIR_WEST, _DIR_SOUTH, _DIR_NORTH }; 
		Vector2[] dirPos = new Vector2[4]{ new Vector2(_x + 1, _y), new Vector2(_x - 1, _y), new Vector2(_x, _y - 1), new Vector2(_x, _y + 1) }; 
		byte dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			// touch the door
			if (CheckWall(ref dir, constDir[i], dirPos[i]) == 0)
			{
				UIManager._Instance.SetTipInfo((isMoved ? "" : Tip._cannotMove) + Tip.GetShowTips(dir, true)); 
				if (GameData._IsShowedOpenDoorTutorial
					&& GameData._IsOpenTutorial)
				{
					GameData._IsShowedOpenDoorTutorial = false; 
					UIManager._Instance.SetSysMsgInfo(SystemMessage._tutorialString_OpenDoor); 
				}
				return; 
			}
		}
		UIManager._Instance.SetTipInfo((isMoved ? "" : Tip._cannotMove) + Tip.GetShowTips(dir)); 

		dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			CheckPit(ref dir, constDir[i], dirPos[i]); 
		}
		string pitString = Tip.GetPitTips(dir); 
		if (!(string.IsNullOrEmpty(pitString)))
		{
			UIManager._Instance.SetTipInfo(pitString); 
		}

		dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			CheckEnemy(ref dir, constDir[i], dirPos[i]); 
		}
		string enemyString = Tip.GetEnemyTips(dir); 
		if (!(string.IsNullOrEmpty(enemyString)))
		{
			UIManager._Instance.SetTipInfo(enemyString); 
		}
	}

	int CheckWall(ref byte dir, byte constDir, Vector2 pos)
	{
		int curIndex = MapManager.CurIndex((int)pos.x, (int)pos.y); 
		if (curIndex < 0 || MapManager._curMap[curIndex] == '1')
		{
			dir |= constDir; 
			if (curIndex < 0)
			{
				dir = 0; 
				dir |= constDir; 
				return 0; // 摸到门
			}
			return 1; // 墙壁
		}
		return -1; // 空气
	}


	#endregion



	#region SwitchLevel

	bool _isUpstairs;

	void InitLevel()
	{
		_isUpstairs = true;
	}

	void NextLevel(int nextLevel)
	{
		if (nextLevel > GameData._MaxLevel || nextLevel <= 0)
		{
			return; 
		}
		if (nextLevel > GameData._CurLevel)
		{
			_isUpstairs = true; 
		}
		else
		{
			_isUpstairs = false; 
		}
		GameData._CurLevel = nextLevel; 
		ChangeStep(GameData._Step + 1); 
		Reset(); 
		if (GameData._CurLevel == 2 && !GameData._CanRotateCamera)
		{
			UIManager._Instance.SetSysMsgInfo(SystemMessage._getLost); 
            GameData._CanRotateCamera = true; 
		}

		if (GameData._CurLevel == 3 && UIManager._Instance._MaxTipCount == 12) // MaTipCount应该放到GameData
		{
			UIManager._Instance.SetSysMsgInfo(SystemMessage._beExhausted); 
			UIManager._Instance._MaxTipCount /= 2; 
		}
		if (GameData._curMeetHint == -1 || GameData._curMeetHint == 0)
		{
			GameData._curMeetHint = 0; 
			if (GameData._CurLevel == 4 && _isUpstairs)
			{ 
				UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Up);
			}
			else if(GameData._CurLevel == 3 && !_isUpstairs)
			{
				UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Down); 
			}
		}

	}

	#endregion



	#region hitPoints

	[SerializeField] Transform _hitPointPrefab;
	Transform _hpParent;
	const int _MaxHP = 5;
	const float _hpDisplayGap = 0.17f;
	int _curHP = 5;

	void InitHP()
	{
		ResetHP(); 
	}

	void ResetHP()
	{
		SetHP(_MaxHP); 
	}

	bool SetHP(int hp)
	{
		Debug.Log("SetHP: " + hp); 
		if (hp > _MaxHP)
		{
			return false; 
		}
		if (hp <= 0)
		{
			Die(); 
			return true; 
		}
		if (_hpParent != null)
		{
			GameObject.Destroy(_hpParent.gameObject); 
		}
		_hpParent = (new GameObject()).transform; 
		_hpParent.name = "HP Parent"; 
		_hpParent.SetParent(transform); 
		_hpParent.localPosition = Vector3.zero; 
		_hpParent.localEulerAngles = Vector3.zero; 
		float x = 0 - (int)(_MaxHP / 2f) * _hpDisplayGap; 
		float y = 0.35f; 
		for (int i = 0; i < hp; ++i)
		{
			Transform tf = GameObject.Instantiate(_hitPointPrefab); 
			tf.SetParent(_hpParent); 
			tf.localPosition = new Vector3(x, y); 
			x += _hpDisplayGap; 
		}
		_curHP = hp; 
		return true; 
	}

	public bool MinusHP(int value)
	{
		return SetHP(_curHP - value); 
	}
	#endregion


	#region Die
	[NonSerialized] public Action _OnClear;
	[NonSerialized] public Action _OnReset; 

	void Die()
	{
		_OnClear(); 
		InitLevel(); 
		_OnReset(); 
	}

	#endregion


	#region Convers
	IEnumerator _meetRoutine; 
	Action _onMeetFinish; 

	IEnumerator MeetRoutine(ERole ident, Action<string> onPlot)
	{
		PlotConf conf = GameData._Instance._plot_Meet; 
		if(conf._triggerRoleIdent != ident)
		{
			yield break; 
		}
		List<SingleConvers> converses = new List<SingleConvers>(conf.conversList); 
		while (converses.Count >= 0)
		{
			RoleConf role = GameData._Instance._roleLib.GetRole(converses[0]._roleIdent); 
			if (role == null)
			{
				converses.RemoveAt(0); 
				continue; 
			}

			string s = role._name + "：" + converses[0]._convers; 
			if(onPlot != null)
			{
				onPlot(s); 
			}

			// there is no need to wait if the length of conversation is zero. 
			converses.RemoveAt(0); 
			if (converses.Count == 0)
			{
				ClearMeetRoutine(); 
				break; 
			}
			yield return new WaitForSeconds(GameData._Instance._conversSpeed); 
		}
	}

	void ClearMeet()
	{
		if (_meetRoutine != null)
		{
			CoroutineUtil.Stop(_meetRoutine); 
			ClearMeetRoutine(); 
		}
	}

	void ClearMeetRoutine()
	{
		if (_onMeetFinish != null)
		{
			_onMeetFinish(); 
			_onMeetFinish = null; 
		}
		_meetRoutine = null; 
	}
	#endregion
}
