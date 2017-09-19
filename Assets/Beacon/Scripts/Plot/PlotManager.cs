using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum EPlotStatus
{
	Start, 
	Meet_Before, 
	Meet, 
	Battle, 
	End, 
}

public class PlotManager : MonoBehaviour
{
	public static PlotManager instance{ private set; get; } 

	public static EPlotStatus status; 
//	public static int curProgress; 
	void Awake()
	{
		instance = this; 
	}

	public void Meet(ERole roleIdent, Action onFinish)
	{
		_onPlotFinish = () =>
			{
				if(onFinish != null)
				{
					onFinish(); 
				}
				UIManager._Instance.SetSysMsgInfo(SystemMessage._openLightMode); 
				_addPlayerRoutine = AddPlayerRoutine(roleIdent);
				StartCoroutine(_addPlayerRoutine); 
				Player._Instance.transform.eulerAngles = Vector3.zero;
				GameData._CanRotateCamera = false; 
				UIManager._Instance.SetMaskEnable(false); 
				PlotManager.status = EPlotStatus.Meet; 
			}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_Meet); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void Battle(Action onFinish)
	{
		_onPlotFinish = () =>
			{
				if(onFinish != null)
				{
					onFinish(); 
				}
				UIManager._Instance.SetMaskEnable(true); 
				UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._removePlayer, GameData._Instance._roleLib.GetRole(ERole.GrandDaughter)._name)); 
				PlotManager.status = EPlotStatus.Battle; 
			}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_Battle); 
		CoroutineUtil.Start(_plotRoutine);
	}

	public void _Start(Action onFinish)
	{
		UIManager._Instance.SetMaskEnable(true); 
		_onPlotFinish = () =>
			{
				if(onFinish != null)
				{
					onFinish(); 
				}
				//				UIManager._Instance.SetMaskEnable(false); 
				PlotManager.status = EPlotStatus.Start; 
			}; 
		_plotRoutine = PlotRoutine(ERole.None, UIManager._Instance.SetTipInfo, GameData._Instance._plot_Start); 
		//		CoroutineUtil.Start(_plotRoutine); 

		_onPlotFinish(); 
	}

	IEnumerator _plotRoutine;
	Action _onPlotFinish;
	IEnumerator _addPlayerRoutine; 

	IEnumerator PlotRoutine(ERole ident, Action<string> onPlot, PlotConf conf)
	{
		if (conf._triggerRoleIdent != ident)
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

			string s = role._name + (string.IsNullOrEmpty(role._name) ? "" : "：") + converses[0]._convers; 
			if (onPlot != null)
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
		if (_plotRoutine != null)
		{
			CoroutineUtil.Stop(_plotRoutine); 
			ClearMeetRoutine(); 
		}
		if (_addPlayerRoutine != null)
		{
			CoroutineUtil.Stop(_addPlayerRoutine); 
			ClearAddPlayerRoutine(); 
		}
	}

	void ClearMeetRoutine()
	{
		if (_onPlotFinish != null)
		{
			_onPlotFinish(); 
			_onPlotFinish = null; 
		}
		_plotRoutine = null; 
	}

	IEnumerator AddPlayerRoutine(ERole roleIdent)
	{
		Debug.Log("add player"); 
		yield return new WaitForSeconds(3); 
		UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._addPlayer, GameData._Instance._roleLib.GetRole(roleIdent)._name)); 
	}

	void ClearAddPlayerRoutine()
	{
		_addPlayerRoutine = null; 
	}
}
