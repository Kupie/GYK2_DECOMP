using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
[Serializable]
public class IndoorAreaBoundData
{
	// Token: 0x06000AB8 RID: 2744 RVA: 0x00036320 File Offset: 0x00034520
	public IndoorAreaBoundData(BoxCollider boxCollider)
	{
		Bounds bounds = boxCollider.bounds;
		this.center = bounds.center;
		this.size = bounds.size;
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00036354 File Offset: 0x00034554
	public bool ContainsXZ(Vector3 worldPos)
	{
		Vector3 vector = this.size * 0.5f;
		return Mathf.Abs(worldPos.x - this.center.x) <= vector.x && Mathf.Abs(worldPos.z - this.center.z) <= vector.z;
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x000363B5 File Offset: 0x000345B5
	public float GetXZArea()
	{
		return Mathf.Abs(this.size.x * this.size.z);
	}

	// Token: 0x04000C31 RID: 3121
	public Vector3 center;

	// Token: 0x04000C32 RID: 3122
	public Vector3 size;
}
