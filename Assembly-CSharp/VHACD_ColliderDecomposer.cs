using System;
using System.Collections.Generic;
using MeshProcess;
using UnityEngine;
using UnityEngine.ProBuilder;

// Token: 0x02000B0E RID: 2830
[RequireComponent(typeof(MeshCollider))]
public class VHACD_ColliderDecomposer : VHACD
{
	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x06004B5A RID: 19290 RVA: 0x00163F33 File Offset: 0x00162133
	public bool IsValid
	{
		get
		{
			return this.decomposedMeshColliderObj != null && this.decomposedMeshColliders.Count > 0;
		}
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x00163F54 File Offset: 0x00162154
	private void Awake()
	{
		if (!this.IsValid)
		{
			return;
		}
		if (this.mainMeshCollider)
		{
			this.mainMeshCollider.sharedMesh = null;
			this.mainMeshCollider.enabled = false;
		}
		MeshRenderer meshRenderer;
		if (this.mainMeshFilter != null && !base.TryGetComponent<MeshRenderer>(out meshRenderer))
		{
			global::UnityEngine.Object.Destroy(this.mainMeshFilter);
		}
		if (this.mainPolyShape != null)
		{
			global::UnityEngine.Object.Destroy(this.mainPolyShape);
		}
	}

	// Token: 0x04003CB3 RID: 15539
	public const string DecomposedConvexMeshColliderName = "DecomposedConvexMeshCollider";

	// Token: 0x04003CB4 RID: 15540
	[SerializeField]
	private MeshCollider mainMeshCollider;

	// Token: 0x04003CB5 RID: 15541
	[SerializeField]
	private MeshFilter mainMeshFilter;

	// Token: 0x04003CB6 RID: 15542
	[SerializeField]
	private PolyShape mainPolyShape;

	// Token: 0x04003CB7 RID: 15543
	[SerializeField]
	private GameObject decomposedMeshColliderObj;

	// Token: 0x04003CB8 RID: 15544
	[SerializeField]
	private List<MeshCollider> decomposedMeshColliders = new List<MeshCollider>();
}
