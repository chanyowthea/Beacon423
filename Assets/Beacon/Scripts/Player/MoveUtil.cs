using UnityEngine;
using System.Collections;

public enum EDirection
{
	East,
	West,
	South,
	North
}

public static class MoveUtil
{
	public const byte _DIR_EAST = 1;
	public const byte _DIR_WEST = 2;
	public const byte _DIR_SOUTH = 4;
	public const byte _DIR_NORTH = 8; 
}
