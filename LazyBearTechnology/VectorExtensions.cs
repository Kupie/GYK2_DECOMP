using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000177 RID: 375
	public static class VectorExtensions
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x00029045 File Offset: 0x00027245
		public static Vector2 SetX(this Vector2 v, float newValue)
		{
			return new Vector2(newValue, v.y);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00029053 File Offset: 0x00027253
		public static Vector2 SetY(this Vector2 v, float newValue)
		{
			return new Vector2(v.x, newValue);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00029061 File Offset: 0x00027261
		public static Vector3 SetX(this Vector3 v, float newValue)
		{
			return new Vector3(newValue, v.y, v.z);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00029075 File Offset: 0x00027275
		public static Vector3 SetY(this Vector3 v, float newValue)
		{
			return new Vector3(v.x, newValue, v.z);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00029089 File Offset: 0x00027289
		public static Vector3 SetZ(this Vector3 v, float newValue)
		{
			return new Vector3(v.x, v.y, newValue);
		}
	}
}
