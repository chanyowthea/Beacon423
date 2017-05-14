using UnityEngine;
using System.Collections;
using System.Linq; 

public class RoleLibrary : ScriptableObject
{
	[SerializeField] RoleConf[] roles; 
	public RoleConf GetRole(ERole ident)
	{
		return roles.Single(x => x._roleIdentity == ident); 
	}
}
