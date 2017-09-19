using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	#region Main

	public static Player _Instance; // 只有高层管理器可以是单例
	[SerializeField] PlayerMove _playerMove; 
	[SerializeField] PlayerHurt _playerHurt; 
	[SerializeField] PlayerDie _playerDie; 


	void Awake()
	{
		_Instance = this; 
	}

	public void Init()
	{
//		Debug.Log("isUpstairs: " + _isUpstairs); 
		LevelManager.instance.InitLevel(); 
		Reset(); 
		_playerHurt.InitHP(); 
		_playerMove.onMinusHP = (int value) =>
		{
			_playerHurt.MinusHP(value); 
		}; 

		_playerMove.isLockMove = true; 
		PlotManager.instance._Start(() =>
			{
				_playerMove.isLockMove = false; 
			}); 
	}

	public void Reset()
	{
		UIManager._Instance.Reset(); 
		UIManager._Instance.SetStep(GameData._Step); 

		// 重置地图数据
		MapManager.DestroyWall(); 
		MapManager.LoadMap(); 
		MapManager.GenerateWall(); 

		// 重置玩家数据
		_playerMove._isUpstairs = LevelManager.instance.isUpStair; 
		_playerMove.ResetPos(); 
		UIManager._Instance.SetCurLevel(GameData._CurLevel); 
		_playerHurt.ResetHP(); 
		SetRoleInfo(ERole.Grandpa); 
	}

	public void Clear()
	{
		ClearRoleInfo(); 
//		ClearMeet(); 
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

	#region Die

	[NonSerialized] public Action _OnClear;
	[NonSerialized] public Action _OnReset;

//	void Die()
//	{
//		_OnClear(); 
//		InitLevel(); 
//		_OnReset(); 
//	}
//
	#endregion

	public Pos GetPos()
	{
		return _playerMove.GetPos(); 
	}

	public bool MinusHP(int value)
	{
		return _playerHurt.MinusHP(value); 
	}

	public void ChangeStep(int step)
	{
		_playerMove.ChangeStep(step); 
	}
}
