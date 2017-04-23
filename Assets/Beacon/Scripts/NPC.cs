using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class NPC : MonoBehaviour 
{
	List<string> _conversations; 

	public string GetConvers()
	{
		if (_conversations == null || _conversations.Count <= 0)
		{
			return; 
		}
		string s = _conversations[0]; 
		_conversations.RemoveAt(0); 
		return s; 
	}
}
