using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000DE RID: 222
	public static class DebugDraw
	{
		// Token: 0x060003CC RID: 972 RVA: 0x00014B84 File Offset: 0x00012D84
		public static void DrawCross(Vector3 p, float size, Color color, float duration = 1f, bool xyPlane = true)
		{
			float num = (float)(xyPlane ? 1 : 0);
			float num2 = 1f - num;
			Debug.DrawLine(p + new Vector3(-size, 0f), p + new Vector3(size, 0f), color, duration);
			Debug.DrawLine(p + new Vector3(0f, -size * num, -size * num2), p + new Vector3(0f, size * num, size * num2), color, duration);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00014C04 File Offset: 0x00012E04
		public static void DrawSquare(Vector3 center, float size, Color color, float duration = 1f, bool xyPlane = true)
		{
			float num = size * 0.5f;
			float num2 = (float)(xyPlane ? 1 : 0);
			float num3 = 1f - num2;
			Vector3 vector = center + new Vector3(-num, -num * num2, -num * num3);
			Vector3 vector2 = center + new Vector3(-num, num * num2, num * num3);
			Vector3 vector3 = center + new Vector3(num, num * num2, num * num3);
			Vector3 vector4 = center + new Vector3(num, -num * num2, -num * num3);
			Debug.DrawLine(vector, vector2, color, duration);
			Debug.DrawLine(vector2, vector3, color, duration);
			Debug.DrawLine(vector3, vector4, color, duration);
			Debug.DrawLine(vector4, vector, color, duration);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014CAC File Offset: 0x00012EAC
		public static void DrawBox(Vector3 center, Vector3 halfExtends, Color color, float duration = 1f)
		{
			Vector3 vector = center + new Vector3(halfExtends.x, halfExtends.y, halfExtends.z);
			Vector3 vector2 = center + new Vector3(-halfExtends.x, halfExtends.y, halfExtends.z);
			Vector3 vector3 = center + new Vector3(halfExtends.x, halfExtends.y, -halfExtends.z);
			Vector3 vector4 = center + new Vector3(-halfExtends.x, halfExtends.y, -halfExtends.z);
			Vector3 vector5 = center + new Vector3(halfExtends.x, -halfExtends.y, halfExtends.z);
			Vector3 vector6 = center + new Vector3(-halfExtends.x, -halfExtends.y, halfExtends.z);
			Vector3 vector7 = center + new Vector3(halfExtends.x, -halfExtends.y, -halfExtends.z);
			Vector3 vector8 = center + new Vector3(-halfExtends.x, -halfExtends.y, -halfExtends.z);
			Debug.DrawLine(vector, vector2, color, duration);
			Debug.DrawLine(vector2, vector4, color, duration);
			Debug.DrawLine(vector3, vector4, color, duration);
			Debug.DrawLine(vector3, vector, color, duration);
			Debug.DrawLine(vector5, vector6, color, duration);
			Debug.DrawLine(vector6, vector8, color, duration);
			Debug.DrawLine(vector7, vector8, color, duration);
			Debug.DrawLine(vector7, vector5, color, duration);
			Debug.DrawLine(vector5, vector, color, duration);
			Debug.DrawLine(vector6, vector2, color, duration);
			Debug.DrawLine(vector7, vector3, color, duration);
			Debug.DrawLine(vector8, vector4, color, duration);
		}
	}
}
