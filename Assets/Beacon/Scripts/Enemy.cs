using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	IEnumerator _updateRoutine; 

	public void Init(Pos pos)
	{
		_x = pos._x; 
		_y = pos._y; 
		if (_updateRoutine != null)
		{
			StopCoroutine(_updateRoutine); 
			_updateRoutine = null; 
		}
		_updateRoutine = UpdateRoutine(); 
		StartCoroutine(_updateRoutine);
		InitHP(); 
	}

	void Clear()
	{
		if (_updateRoutine != null)
		{
			StopCoroutine(_updateRoutine); 
			_updateRoutine = null; 
		}
	}

	IEnumerator UpdateRoutine()
	{
		System.Random random = new System.Random(); 
		Pos[] coordinates = new Pos[4]{new Pos(-1, 0), new Pos(1, 0), new Pos(0, -1), new Pos(0, 1)}; 

		while(true)
		{
			yield return null; 
			if ((int)(random.Next(50)) == 0)
//			if(Input.GetKey(KeyCode.Space))
			{
				Debug.Log("Enemy Move"); 

				int index = random.Next(0, 4); 
				Pos pos = coordinates[index]; 
				Move(pos._x, pos._y); 
			}
		}
	}

	void OnDestroy()
	{
		Clear(); 
	}
	#region hitPoints

//	[SerializeField] Transform _hitPointPrefab;
	Transform _hpParent;
	const int _MaxHP = 2;
	const float _hpDisplayGap = 0.17f;
	int _curHP = 5;

	void InitHP()
	{
		SetHP(_MaxHP); 
	}

	public bool SetHP(int hp)
	{
		Debug.Log("Enemy SetHP: " + hp); 
		if (hp > _MaxHP)
		{
			return false; 
		}
		if (hp <= 0)
		{
			Die(); 
			return true; 
		}
		if (_hpParent != null)
		{
			GameObject.Destroy(_hpParent.gameObject); 
		}
		_hpParent = (new GameObject()).transform; 
		_hpParent.name = "HP Parent"; // TODO need to be optimized
		_hpParent.SetParent(transform); 
		_hpParent.localPosition = Vector3.zero; 
		_hpParent.localEulerAngles = Vector3.zero; 
		float x = 0 - (int)(_MaxHP / 2f) * _hpDisplayGap; 
		float y = 0.35f; 
		for (int i = 0; i < hp; ++i)
		{
			Transform tf = GameObject.Instantiate(GameData._Instance._hitPointPrefab); 
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
	#endregion


	#region Die
	void Die()
	{
		Debug.Log("Die: " + transform.name); 
		int index = MapManager.CurIndex(_x, _y); 
		if (index >= 0)
		{
			MapManager.ResetMap(index); 
		}
	}

	#endregion



	#region Move
	int _x; 
	int _y; 
	void Move(int x, int y) // 3 Enemy, 2 Pit, 1 Wall, 0 Road, 
	{
		int newX = _x + (int)x; 
		int newY = _y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

		if (curIndex >= 0)
		{
			Debug.Log("x, y: " + x + ", " + y); 
//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 


			if (Pos.IsIdentical(Player._Instance.GetPos(), new Pos(newX, newY))) // MapManager._curMap[curIndex] == MapCode.PIT
			{
				Player player = Player._Instance; 
				int count = 1; 
				bool rs = player.MinusHP(count); 
				if (rs)
				{
					UIManager._Instance.SetSysMsgInfo(string.Format("你受到{0}点伤害！", count)); // TODO 要设置敌人的攻击力，玩家的防御力
				}
			}
			else if (MapManager._curMap[curIndex] == MapCode.NONE || MapManager._curMap[curIndex] == MapCode.BEFORE_UPSTAIR 
				|| MapManager._curMap[curIndex] == MapCode.BEFORE_DOWNSTAIR)
			{
				int index = MapManager.CurIndex(_x, _y);
				_x = newX; 
				_y = newY; 

				transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0); 

				if (index >= 0)
				{
					MapManager.ResetMap(curIndex, index);
				}
				else
				{
					Debug.Log("Index is minus! "); 
				}
			}
		}
	}

	#endregion
}
