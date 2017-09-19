using UnityEngine;
using System.Collections;
using System; 

public class BaseMove : MonoBehaviour
{
	public virtual int x{get {return _x; }}
	public virtual int y{get {return _y; }}
	[SerializeField] protected int _x;
	[SerializeField] protected int _y; 

	public bool isLockMove;

	public virtual void ResetPos()
	{
		
	}



	protected virtual void Move(int x, int y)
	{
		
	}

	public virtual Pos GetPos()
	{
		return new Pos(_x, _y); 
	}
}