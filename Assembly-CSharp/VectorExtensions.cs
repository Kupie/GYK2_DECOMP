using System;
using UnityEngine;

// Token: 0x02000ACD RID: 2765
public static class VectorExtensions
{
	// Token: 0x06004AA6 RID: 19110 RVA: 0x0016056B File Offset: 0x0015E76B
	public static Vector3 XZ(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}

	// Token: 0x06004AA7 RID: 19111 RVA: 0x00160583 File Offset: 0x0015E783
	public static Vector2 XZ2(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x00160596 File Offset: 0x0015E796
	public static Vector3 XZ(this Vector2 vector)
	{
		return new Vector3(vector.x, 0f, vector.y);
	}
}
