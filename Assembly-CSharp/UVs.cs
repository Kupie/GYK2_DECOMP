using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
[Serializable]
public class UVs
{
	// Token: 0x06004B13 RID: 19219 RVA: 0x001621C1 File Offset: 0x001603C1
	public UVs(int capacity)
	{
		this.uvs = new List<Vector4>(capacity);
	}

	// Token: 0x04003C93 RID: 15507
	public List<Vector4> uvs = new List<Vector4>();
}
