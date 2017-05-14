using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum PlotStatus
{
	Meet_Before, 
	Meet, 
}

public class PlotManager : MonoBehaviour
{
//	public static IEnumerator Meet(ERole ident, Action<string> onPlot, Action onFinish)
//	{
//		PlotConf conf = GameData._Instance._plot_Meet; 
//		if(conf._triggerRoleIdent != ident)
//		{
//			yield break; 
//		}
//		List<SingleConvers> converses = new List<SingleConvers>(conf.conversList); 
//		while (converses.Count >= 0)
//		{
//			RoleConf role = GameData._Instance._roleLib.GetRole(converses[0]._roleIdent); 
//			if (role == null)
//			{
//				converses.RemoveAt(0); 
//				continue; 
//			}
//
//			string s = role._name + "：" + converses[0]._convers; 
//			if(onPlot != null)
//			{
//				onPlot(s); 
//			}
//
//			// there is no need to wait if the length of conversation is zero. 
//			converses.RemoveAt(0); 
//			if (converses.Count == 0)
//			{
//				onFinish(); 
//				break; 
//			}
//			yield return new WaitForSeconds(GameData._Instance._conversSpeed); 
//		}
//	}
}
