using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
[Serializable]
public struct WorldZoneNavigationHoleBakedData
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x0003B5C8 File Offset: 0x000397C8
	public static WorldZoneNavigationHoleBakedData FromBoxCollider(BoxCollider boxCollider)
	{
		Vector3 lossyScale = boxCollider.transform.lossyScale;
		Vector3 vector = Vector3.Scale(boxCollider.size, lossyScale);
		return new WorldZoneNavigationHoleBakedData
		{
			center = boxCollider.transform.TransformPoint(boxCollider.center),
			size = new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z)),
			rotation = boxCollider.transform.rotation
		};
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0003B650 File Offset: 0x00039850
	public WorldZoneNavigationHoleBakedData WithOffset(Vector3 offset)
	{
		return new WorldZoneNavigationHoleBakedData
		{
			center = this.center + offset,
			size = this.size,
			rotation = this.rotation
		};
	}

	// Token: 0x04000CC6 RID: 3270
	public Vector3 center;

	// Token: 0x04000CC7 RID: 3271
	public Vector3 size;

	// Token: 0x04000CC8 RID: 3272
	public Quaternion rotation;
}
