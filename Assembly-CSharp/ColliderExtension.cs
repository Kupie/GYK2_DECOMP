using System;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public static class ColliderExtension
{
	// Token: 0x0600498F RID: 18831 RVA: 0x0015B7EC File Offset: 0x001599EC
	public static bool IsLossyScaleNegative(this Collider col)
	{
		return col.transform.lossyScale.x < 0f || col.transform.lossyScale.y < 0f || col.transform.lossyScale.z < 0f;
	}

	// Token: 0x06004990 RID: 18832 RVA: 0x0015B840 File Offset: 0x00159A40
	public static void FixBoxColliderLossyScale(this BoxCollider boxCol)
	{
		Transform transform = boxCol.transform;
		Vector3 localScale = transform.localScale;
		Vector3 center = boxCol.center;
		Vector3 size = boxCol.size;
		if (transform.lossyScale.x < 0f)
		{
			localScale.x *= -1f;
			center.x *= -1f;
			size.x *= -1f;
		}
		if (transform.lossyScale.y < 0f)
		{
			localScale.y *= -1f;
			center.y *= -1f;
			size.y *= -1f;
		}
		if (transform.lossyScale.z < 0f)
		{
			localScale.z *= -1f;
			center.z *= -1f;
			size.z *= -1f;
		}
		transform.localScale = localScale;
		boxCol.size = size;
		boxCol.center = center;
	}
}
