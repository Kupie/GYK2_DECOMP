using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000187 RID: 391
	public static class MathUtilities
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0002A4A7 File Offset: 0x000286A7
		public static Vector2 RadianToVector2(float radian)
		{
			return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002A4BA File Offset: 0x000286BA
		public static Vector2 AngleToVector2(float angle)
		{
			return MathUtilities.RadianToVector2(angle * 0.017453292f);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002A4C8 File Offset: 0x000286C8
		public static int ClampCycle(int value, int min, int max)
		{
			if (value > max)
			{
				return min;
			}
			if (value < min)
			{
				return max;
			}
			return value;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002A4D7 File Offset: 0x000286D7
		public static float ClampCycle(float value, float min, float max)
		{
			if (value > max)
			{
				return min;
			}
			if (value < min)
			{
				return max;
			}
			return value;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002A4E6 File Offset: 0x000286E6
		public static float ClampCycleWithinRange(int value, int min, int max)
		{
			if (value > max)
			{
				return (float)(min + Mathf.Clamp(value - max, min, max));
			}
			if (value < min)
			{
				return (float)(max - Mathf.Clamp(min - value, min, max));
			}
			return (float)value;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002A50E File Offset: 0x0002870E
		public static float ClampCycleWithinRange(float value, float min, float max)
		{
			if (value > max)
			{
				return min + Mathf.Clamp(value - max, min, max);
			}
			if (value < min)
			{
				return max - Mathf.Clamp(min - value, min, max);
			}
			return value;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0002A533 File Offset: 0x00028733
		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
		}
	}
}
