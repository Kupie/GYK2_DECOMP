using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class MultiObject3DAnimatable : MonoBehaviour
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x0600216B RID: 8555 RVA: 0x0009DA0F File Offset: 0x0009BC0F
	public List<Object3D> Object3Ds
	{
		get
		{
			return this.object3Ds;
		}
	}

	// Token: 0x04001E01 RID: 7681
	[SerializeField]
	private List<Object3D> object3Ds = new List<Object3D>();
}
