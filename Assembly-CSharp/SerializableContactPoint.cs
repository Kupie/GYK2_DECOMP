using System;
using UnityEngine;

// Token: 0x02000428 RID: 1064
[Serializable]
public class SerializableContactPoint
{
	// Token: 0x06001C1B RID: 7195 RVA: 0x00082ECE File Offset: 0x000810CE
	public SerializableContactPoint(Vector3 point, Vector3 normal)
	{
		this.point = point;
		this.normal = normal;
	}

	// Token: 0x04001A95 RID: 6805
	public Vector3 point;

	// Token: 0x04001A96 RID: 6806
	public Vector3 normal;
}
