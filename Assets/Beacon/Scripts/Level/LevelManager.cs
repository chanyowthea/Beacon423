using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance{ private set; get; }

	public bool isUpStair{ get { return _isUpstairs; }}
	bool _isUpstairs;

	void Awake()
	{
		instance = this; 
	}

	public void InitLevel()
	{
		_isUpstairs = true;
	}

	public void NextLevel(int nextLevel)
	{
		Debug.LogError("next: " + nextLevel); 
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
		Player._Instance.ChangeStep(GameData._Step + 1); 
		Player._Instance.Reset(); 
		if (GameData._CurLevel == 2 && !GameData._CanRotateCamera && GameData._isOpenMask)
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
			if (GameData._CurLevel == 4 && _isUpstairs)
			{ 
				GameData._curMeetHint = 0; 
				UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Up);
			}
			else if (GameData._CurLevel == 3 && !_isUpstairs)
			{
				GameData._curMeetHint = 0; 
				UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Down); 
			}
		}

	}
}
