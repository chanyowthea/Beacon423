using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class Convers
{
	public string _convers_NPC; 
	public string _convers_Player; 

	public Convers(string s1, string s2)
	{
		_convers_NPC = s1; 
		_convers_Player = s2; 
	}
}

public class NPC : MonoBehaviour 
{
	public ERole _roleIdent; 
	Sprite[] _moveSprites; 
	[SerializeField] TextMesh _nameText; 
	public void Init(ERole roleIdent, Sprite[] moveSprites)
	{
		_roleIdent = roleIdent; 
		_moveSprites = moveSprites; 

		RoleConf role = GameData._Instance._roleLib.GetRole(roleIdent); 
		if (role != null)
		{ 
			_nameText.text = role._name; 
		}
	}

	public void Clear()
	{
		_roleIdent = ERole.Grandpa; 
		_moveSprites = null; 
		_nameText.text = null; 
	}
}
