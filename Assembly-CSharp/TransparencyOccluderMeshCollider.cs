using System;
using UnityEngine;

// Token: 0x0200052D RID: 1325
[ExecuteAlways]
public class TransparencyOccluderMeshCollider : MonoBehaviour
{
	// Token: 0x0600220C RID: 8716 RVA: 0x0009FE94 File Offset: 0x0009E094
	private void ApplyParameters()
	{
		base.gameObject.layer = 15;
		if (!this.meshFilter)
		{
			return;
		}
		this.meshCollider.sharedMesh = this.meshFilter.sharedMesh;
	}

	// Token: 0x04001EA6 RID: 7846
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04001EA7 RID: 7847
	[SerializeField]
	private MeshCollider meshCollider;
}
