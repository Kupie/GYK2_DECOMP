using System;
using UnityEngine;

// Token: 0x02000AC8 RID: 2760
public static class DirectionExtensions
{
	// Token: 0x06004A94 RID: 19092 RVA: 0x00160268 File Offset: 0x0015E468
	public static Vector3 ConvertToVector3(this Direction direction)
	{
		switch (direction)
		{
		case Direction.Right:
			return Vector3.right.normalized;
		case Direction.Up:
			return Vector3.forward.normalized;
		case Direction.Left:
			return Vector3.left.normalized;
		case Direction.Down:
			return Vector3.back.normalized;
		default:
			Debug.LogWarning(string.Format("DirectionExtensions Error: no direction [{0}] setup.", direction));
			return Vector3.zero;
		}
	}

	// Token: 0x06004A95 RID: 19093 RVA: 0x001602E1 File Offset: 0x0015E4E1
	public static Direction ConvertFromVector2(this Vector2 direction)
	{
		return BasicNpcSteppedRotationPreset.Instance.ComputeAngle(direction).ConvertFromSignedAngle();
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x001602F3 File Offset: 0x0015E4F3
	public static Direction ConvertFromVector3(this Vector3 direction)
	{
		return direction.XZ2().ConvertFromVector2();
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x00160300 File Offset: 0x0015E500
	public static Vector2 ConvertToVector2XZ(this Direction direction)
	{
		Vector3 vector = direction.ConvertToVector3();
		return new Vector2(vector.x, vector.z);
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x00160325 File Offset: 0x0015E525
	public static Direction ClockwiseDir(this Direction dir)
	{
		switch (dir)
		{
		case Direction.Right:
			return Direction.Down;
		case Direction.Up:
			return Direction.Right;
		case Direction.Left:
			return Direction.Up;
		case Direction.Down:
			return Direction.Left;
		default:
			return Direction.None;
		}
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x0016034A File Offset: 0x0015E54A
	public static Direction OppositeDir(this Direction dir)
	{
		switch (dir)
		{
		case Direction.Right:
			return Direction.Left;
		case Direction.Up:
			return Direction.Down;
		case Direction.Left:
			return Direction.Right;
		case Direction.Down:
			return Direction.Up;
		default:
			return dir;
		}
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x00160370 File Offset: 0x0015E570
	public static float ConvertToSignedAngle(this Direction direction)
	{
		switch (direction)
		{
		case Direction.Right:
			return 0f;
		case Direction.Up:
			return 90f;
		case Direction.Left:
			return 180f;
		case Direction.Down:
			return -90f;
		default:
			Debug.LogWarning(string.Format("DirectionExtensions Error: no direction [{0}] setup.", direction));
			return 0f;
		}
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x001603C9 File Offset: 0x0015E5C9
	public static Direction ConvertFromSignedAngle(this float angle)
	{
		return BasicNpcSteppedRotationPreset.Instance.GetDirectionFromAngle(angle);
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x001603D6 File Offset: 0x0015E5D6
	public static bool IsValid(this Direction direction)
	{
		return direction == Direction.Up || direction == Direction.Down || direction == Direction.Left || direction == Direction.Right;
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x001603EC File Offset: 0x0015E5EC
	private static Vector2 ConvertVector3DirectionToVector2XZ(this Vector3 direction3D)
	{
		return (Quaternion.AngleAxis(90f, Vector3.right) * new Vector2(direction3D.x, direction3D.y)).normalized;
	}
}
