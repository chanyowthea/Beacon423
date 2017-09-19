using UnityEngine;
using System.Collections;
using System; 

//public class PlayerMove : IMove
//{
//	public void To(GameObject obj, Vector3 pos)
//	{
//		obj.transform.position = pos;
//	}
//}

public class PlayerMove : BaseMove
{
//	public int x{get {return _x; }}
//	public int y{get {return _y; }}
	public bool _isUpstairs; 
	[NonSerialized] public Action<int> onMinusHP; 
//	[SerializeField] int _x;
//	[SerializeField] int _y; 
//
//	public bool isLockMove;

	public override void ResetPos()
	{
		base.ResetPos(); 
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
		float[] angles = new float[]{ -90, 90, 180, 0 }; 
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



	protected override void Move(int x, int y) // 3 Enemy, 2 Pit, 1 Wall, 0 Road, 
	{
		int newX = _x + (int)x; 
		int newY = _y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

		if (curIndex >= 0)
		{
			//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 
			if (MapManager._curMap[curIndex] == '0' || MapManager._curMap[curIndex] == '9' || MapManager._curMap[curIndex] == '8')
			{
				//				int index = MapManager.CurIndex(_x, _y); 
				_x = newX; 
				_y = newY; 
				ChangeStep(GameData._Step + 1);
				CalculateDirection();

				if (PlotManager.status == EPlotStatus.Battle)
				{
					if (MapManager.GetCode(newX + 0, newY + 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你前方一米处"); 
					}
					else if (MapManager.GetCode(newX + 0, newY + 2) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你前方两米处"); 
					}
					else if (MapManager.GetCode(newX + 0, newY - 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你后方一米处"); 
					}
					else if (MapManager.GetCode(newX + 0, newY - 2) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你后方两米处"); 
					}
					else if (MapManager.GetCode(newX + 1, newY + 0) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你右方一米处"); 
					}
					else if (MapManager.GetCode(newX + 2, newY + 0) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你右方两米处"); 
					}
					else if (MapManager.GetCode(newX - 1, newY + 0) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你左方一米处"); 
					}
					else if (MapManager.GetCode(newX - 2, newY + 0) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你左方两米处"); 
					}
					else if (MapManager.GetCode(newX + 1, newY + 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你右前方一米处"); 
					}
					else if (MapManager.GetCode(newX - 1, newY - 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你左后方两米处"); 
					}
					else if (MapManager.GetCode(newX + 1, newY - 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你右后方两米处"); 
					}
					else if (MapManager.GetCode(newX - 1, newY + 1) == MapCode.NPC_DARK_PRINCE)
					{
						UIManager._Instance.SetTipInfo("爷爷，坏人在你左前方两米处"); 
					}
				}

				int enemyX; 
				int enemyY; 
				if(MapManager.IsExistCodeInRange(newX, newY, out enemyX, out enemyY, MapCode.NPC)
					|| MapManager.IsExistCodeInRange(newX, newY, out enemyX, out enemyY, MapCode.NPC_DARK_PRINCE))
				{
					int idx = MapManager.CurIndex(enemyX, enemyY); 
					if (GameData._curMeetHint <= 0 && MapManager._curMap[idx] == MapCode.NPC)
					{
						if (PlotManager.status == EPlotStatus.Start)
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
								PlotManager.instance.Meet(npc._roleIdent, () =>
									{
										isLockMove = false; 
									}); 
							}
						}
					}
					else if (MapManager._curMap[idx] == MapCode.NPC_DARK_PRINCE)
					{
						if (PlotManager.status == EPlotStatus.Meet)
						{
							GameObject go = MapManager.GetObj(idx); 
							NPC npc = null; 
							if (go != null)
							{
								npc = go.GetComponent<NPC>();
							}
							if (npc != null && npc._roleIdent == ERole.DarkPrince)
							{
								isLockMove = true; 
								PlotManager.instance.Battle(() =>
									{
										isLockMove = false; 
									}); 
							}
						}
					}
				}
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
				if (onMinusHP != null)
				{
					onMinusHP(count); 
				}
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
			Debug.LogError("curLevel: " + GameData._CurLevel + ", code: " + MapManager._curMap[MapManager.CurIndex(_x, _y)]); 
			LevelManager.instance.NextLevel(GameData._CurLevel + (MapManager._curMap[MapManager.CurIndex(_x, _y)] == '9' ? 1 : -1)); 
		}
	}

	void CalculateDirection(bool isMoved = true)
	{ 
		Vector2[] dirPos = new Vector2[4]{ new Vector2(_x + 1, _y), new Vector2(_x - 1, _y), 
			new Vector2(_x, _y - 1), new Vector2(_x, _y + 1) }; 
		byte dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			// touch the door
			if (MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.WALL) == 0)
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
			MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.PIT); 
		}
		string pitString = Tip.GetPitTips(dir); 
		if (!(string.IsNullOrEmpty(pitString)))
		{
			UIManager._Instance.SetTipInfo(pitString); 
		}

		dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.ENEMY); 
		}
		string enemyString = Tip.GetEnemyTips(dir); 
		if (!(string.IsNullOrEmpty(enemyString)))
		{
			UIManager._Instance.SetTipInfo(enemyString); 
		}
	}

	public void ChangeStep(int step)
	{
		GameData._Step = step; // 新的x，y必须是可以行走的才加步数 
		UIManager._Instance.SetStep(GameData._Step); 
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
}