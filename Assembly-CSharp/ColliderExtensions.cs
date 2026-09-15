using System;
using UnityEngine;

// Token: 0x02000AC7 RID: 2759
public static class ColliderExtensions
{
	// Token: 0x06004A93 RID: 19091 RVA: 0x0016021C File Offset: 0x0015E41C
	public static Vector3 GetContactPosition(this Collider collider, Vector3 otherPos, float lerpT = 0.5f)
	{
		Vector3 vector = collider.ClosestPoint(otherPos);
		float num = Mathf.Max(vector.y, otherPos.y);
		return Vector3.Lerp(vector, collider.transform.position, lerpT).XZ() + Vector3.up * num;
	}
}
