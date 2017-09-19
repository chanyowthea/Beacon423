using UnityEngine;
using System.Collections;
using System;

public class PlotConf : ScriptableObject
{
	public ERole _triggerRoleIdent; 
    [ArrayAttribute]
	public SingleConvers[] conversList; 
}

[Serializable]
public class SingleConvers
{
	public ERole _roleIdent;
	public string _convers; 
}
