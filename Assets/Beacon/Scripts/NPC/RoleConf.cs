using UnityEngine;
using System.Collections;

public enum ERole
{
	None, 
	Grandpa, 
	GrandDaughter, 
	DarkPrince, 
}

public class RoleConf : ScriptableObject
{
	public int _id; 
	public ERole _roleIdentity; 
	public string _name; 
	public Sprite _avatar; 
	public Sprite[] _moveSprites; 
}
	
