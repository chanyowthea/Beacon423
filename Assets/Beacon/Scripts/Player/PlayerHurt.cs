using UnityEngine;
using System.Collections;
using System; 

public class PlayerHurt : MonoBehaviour
{
	[SerializeField] Transform _hitPointPrefab;
	Transform _hpParent;
	const int _MaxHP = 5;
	const float _hpDisplayGap = 0.17f;
	int _curHP = 5;
	Action onDie; 

	public void InitHP()
	{
		ResetHP(); 
	}

	public void ResetHP()
	{
		SetHP(_MaxHP); 
	}

	public bool SetHP(int hp)
	{
		Debug.Log("SetHP: " + hp); 
		if (hp > _MaxHP)
		{
			return false; 
		}
		if (hp <= 0)
		{
			if (onDie != null)
			{
				onDie(); 
			}
			return true; 
		}
		if (_hpParent != null)
		{
			GameObject.Destroy(_hpParent.gameObject); 
		}
		_hpParent = (new GameObject()).transform; 
		_hpParent.name = "HP Parent"; 
		_hpParent.SetParent(transform); 
		_hpParent.localPosition = Vector3.zero; 
		_hpParent.localEulerAngles = Vector3.zero; 
		float x = 0 - (int)(_MaxHP / 2f) * _hpDisplayGap; 
		float y = 0.35f; 
		for (int i = 0; i < hp; ++i)
		{
			Transform tf = GameObject.Instantiate(_hitPointPrefab); 
			tf.SetParent(_hpParent); 
			tf.localPosition = new Vector3(x, y); 
			x += _hpDisplayGap; 
		}
		_curHP = hp; 
		return true; 
	}

	public bool MinusHP(int value)
	{
		return SetHP(_curHP - value); 
	}
}
