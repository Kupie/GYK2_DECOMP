using System;
using LazyBearTechnology;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020004FB RID: 1275
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DeformingGrass : MonoBehaviour
{
	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06002130 RID: 8496 RVA: 0x0009CA03 File Offset: 0x0009AC03
	public Mesh Mesh
	{
		get
		{
			return this.fullMesh;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06002131 RID: 8497 RVA: 0x0009CA0C File Offset: 0x0009AC0C
	private MaterialPropertyBlock MatProp
	{
		get
		{
			MaterialPropertyBlock materialPropertyBlock;
			if ((materialPropertyBlock = this.matProp) == null)
			{
				materialPropertyBlock = (this.matProp = new MaterialPropertyBlock());
			}
			return materialPropertyBlock;
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06002132 RID: 8498 RVA: 0x0009CA31 File Offset: 0x0009AC31
	private MeshRenderer MeshRenderer
	{
		get
		{
			if (this.meshRenderer == null)
			{
				this.meshRenderer = base.GetComponent<MeshRenderer>();
			}
			return this.meshRenderer;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06002133 RID: 8499 RVA: 0x0009CA53 File Offset: 0x0009AC53
	private MeshFilter MeshFilter
	{
		get
		{
			if (this.meshFilter == null)
			{
				this.meshFilter = base.GetComponent<MeshFilter>();
			}
			return this.meshFilter;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06002134 RID: 8500 RVA: 0x0009CA75 File Offset: 0x0009AC75
	// (set) Token: 0x06002135 RID: 8501 RVA: 0x0009CA7C File Offset: 0x0009AC7C
	public static bool IsEnabled
	{
		get
		{
			return DeformingGrass.isEnabled;
		}
		set
		{
			DeformingGrass[] array = global::UnityEngine.Object.FindObjectsOfType<DeformingGrass>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(value);
			}
			DeformingGrass.isEnabled = value;
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x0009CAB2 File Offset: 0x0009ACB2
	public void ApplyMaterial()
	{
		if (this.atlas == null)
		{
			return;
		}
		this.MatProp.SetTexture(DeformingGrass.idMainTex, this.atlas.texture);
		this.MeshRenderer.SetPropertyBlock(this.MatProp);
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x0009CAEF File Offset: 0x0009ACEF
	private void OnEnable()
	{
		this.ApplyMaterial();
		this.ApplyShadowSettings();
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x0009CAFD File Offset: 0x0009ACFD
	public void ApplyShadowSettings()
	{
		this.MeshRenderer.shadowCastingMode = (LazySingletonSO<DeformingGrassSettings>.Instance.grassShadow ? ShadowCastingMode.TwoSided : ShadowCastingMode.Off);
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x0009CB1C File Offset: 0x0009AD1C
	public void DrawFromData(LazyTerrainMesh lazyTerrainMesh, DeformingGrassData data, LazyTerrain lazyTerrain)
	{
		if (Application.isPlaying)
		{
			LazyTerrainMeshCollection.Runtime_StripCpuMeshData(data.mesh);
		}
		this.MeshFilter.mesh = data.mesh;
		this.fullMesh = data.mesh;
		this.lazyTerrain = lazyTerrain;
		base.transform.SetParent(lazyTerrainMesh.transform);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = quaternion.identity;
	}

	// Token: 0x04001DC4 RID: 7620
	private static readonly int idMainTex = Shader.PropertyToID("_MainTex");

	// Token: 0x04001DC5 RID: 7621
	[SerializeField]
	private Mesh fullMesh;

	// Token: 0x04001DC6 RID: 7622
	public DeformingGrassAtlas atlas;

	// Token: 0x04001DC7 RID: 7623
	public LazyTerrain lazyTerrain;

	// Token: 0x04001DC8 RID: 7624
	private MaterialPropertyBlock matProp;

	// Token: 0x04001DC9 RID: 7625
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04001DCA RID: 7626
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04001DCB RID: 7627
	private static bool isEnabled = true;
}
