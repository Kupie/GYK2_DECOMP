using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class PreSetModuleBuildView : MonoBehaviour
{
	// Token: 0x17000155 RID: 341
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x0002CB3D File Offset: 0x0002AD3D
	public Bounds Bounds
	{
		get
		{
			return new Bounds(base.transform.position, this.size);
		}
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0002CB58 File Offset: 0x0002AD58
	public void SetPositionAndScaleAs(BuildArea buildArea)
	{
		Collider collider = buildArea.Collider;
		if (collider == null)
		{
			return;
		}
		base.transform.position = collider.bounds.center;
		this.size = collider.bounds.size;
		if (this.planeTransform != null)
		{
			this.planeTransform.localScale = collider.bounds.size * 0.1f;
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0002CBD4 File Offset: 0x0002ADD4
	public bool IsFullyInsideIn(Bounds otherBounds)
	{
		Bounds bounds = this.Bounds;
		Vector3 vector = new Vector3(bounds.min.x, otherBounds.center.y, bounds.min.z);
		Vector3 vector2 = new Vector3(bounds.max.x, otherBounds.center.y, bounds.max.z);
		float num = 0.001f;
		bool flag = vector.x >= otherBounds.min.x - num && vector.x <= otherBounds.max.x + num && vector.z >= otherBounds.min.z - num && vector.z <= otherBounds.max.z + num;
		bool flag2 = vector2.x >= otherBounds.min.x - num && vector2.x <= otherBounds.max.x + num && vector2.z >= otherBounds.min.z - num && vector2.z <= otherBounds.max.z + num;
		return flag && flag2;
	}

	// Token: 0x04000A6C RID: 2668
	public Transform planeTransform;

	// Token: 0x04000A6D RID: 2669
	private Vector3 size;
}
