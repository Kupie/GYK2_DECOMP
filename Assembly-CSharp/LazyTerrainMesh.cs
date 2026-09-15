using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020006B9 RID: 1721
public class LazyTerrainMesh : MonoBehaviour
{
	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000DAF4A File Offset: 0x000D914A
	public Mesh Mesh
	{
		get
		{
			return this.meshFilter.sharedMesh;
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06002DC2 RID: 11714 RVA: 0x000DAF57 File Offset: 0x000D9157
	public Vector3 VertexOffset
	{
		get
		{
			return base.transform.localPosition - this.center;
		}
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x000DAF70 File Offset: 0x000D9170
	public void DrawFromData(LazyTerrainMeshData data, LazyTerrain lazyTerrain, DeformingGrass grass = null)
	{
		this.center = data.center;
		this.bounds = data.bounds;
		if (Application.isPlaying)
		{
			LazyTerrainMeshCollection.Runtime_StripCpuMeshData(data.mesh);
		}
		this.meshFilter.sharedMesh = data.mesh;
		this.meshRenderer.sharedMaterial = data.lazyTerrain.material;
		this.meshRenderer.shadowCastingMode = (data.lazyTerrain.castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
		base.transform.SetParent(data.lazyTerrain.transform);
		base.transform.localPosition = data.localPosition;
		if (grass != null)
		{
			grass.DrawFromData(this, data.deformingGrassData, lazyTerrain);
			this.deformingGrass = grass;
		}
	}

	// Token: 0x040024C9 RID: 9417
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x040024CA RID: 9418
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x040024CB RID: 9419
	public Vector3 center;

	// Token: 0x040024CC RID: 9420
	public Bounds bounds;

	// Token: 0x040024CD RID: 9421
	public DeformingGrass deformingGrass;
}
