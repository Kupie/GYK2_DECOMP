using System;
using UnityEngine;

// Token: 0x02000723 RID: 1827
[ExecuteAlways]
public class MeshBoundsUvToShader : MonoBehaviour
{
	// Token: 0x06002FC4 RID: 12228 RVA: 0x000E51CA File Offset: 0x000E33CA
	private void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x000E51CA File Offset: 0x000E33CA
	private void OnValidate()
	{
		this.Apply();
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000E51D4 File Offset: 0x000E33D4
	private void Update()
	{
		if (!this.updateEveryFrame)
		{
			return;
		}
		if (base.transform.lossyScale != this.lastScale || base.transform.position != this.lastPosition)
		{
			this.Apply();
			this.lastScale = base.transform.lossyScale;
			this.lastPosition = base.transform.position;
		}
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000E5244 File Offset: 0x000E3444
	public void SetTextures(Texture2D iconTexture, Texture2D gridTexture)
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.propertyBlock.SetTexture(this.shaderIdIconTexture, iconTexture);
		this.propertyBlock.SetTexture(this.shaderIdGridTexture, gridTexture);
		this.targetRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000E529C File Offset: 0x000E349C
	public void SetCellsCount(Vector2Int cellsCount)
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.targetRenderer.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetVector(this.shaderIdCellsCount, new Vector4((float)cellsCount.x, (float)cellsCount.y, 0f, 0f));
		this.targetRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000E5310 File Offset: 0x000E3510
	public void Apply()
	{
		if (this.targetRenderer == null)
		{
			this.targetRenderer = base.GetComponent<Renderer>();
		}
		if (this.targetMeshFilter == null)
		{
			this.targetMeshFilter = base.GetComponent<MeshFilter>();
		}
		if (this.targetRenderer == null || this.targetMeshFilter == null)
		{
			return;
		}
		Mesh sharedMesh = this.targetMeshFilter.sharedMesh;
		if (sharedMesh == null)
		{
			return;
		}
		Bounds bounds = sharedMesh.bounds;
		Vector3 vector = Vector3.Scale(bounds.size, this.targetRenderer.transform.lossyScale);
		Vector2 worldSizeUv = this.GetWorldSizeUv(vector);
		Vector2 worldSizeUv2 = this.GetWorldSizeUv(bounds.size);
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.targetRenderer.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetVector("_MeshBoundsMin", bounds.min);
		this.propertyBlock.SetVector("_MeshBoundsSize", bounds.size);
		this.propertyBlock.SetVector("_MeshWorldSize", vector);
		this.propertyBlock.SetVector("_MeshWorldSizeUV", new Vector4(worldSizeUv.x, worldSizeUv.y, 0f, 0f));
		this.propertyBlock.SetVector("_MeshBoundsSizeUV", new Vector4(worldSizeUv2.x, worldSizeUv2.y, 0f, 0f));
		this.propertyBlock.SetVector("_MeshUVScale", new Vector4(this.meshUvScale.x, this.meshUvScale.y, 0f, 0f));
		this.propertyBlock.SetVector("_MeshUVOffset", new Vector4(this.meshUvOffset.x, this.meshUvOffset.y, 0f, 0f));
		this.targetRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000E5504 File Offset: 0x000E3704
	private Vector2 GetWorldSizeUv(Vector3 worldSize)
	{
		MeshBoundsUvToShader.MeshUvPlane meshUvPlane = this.meshUvPlane;
		if (meshUvPlane == MeshBoundsUvToShader.MeshUvPlane.XY)
		{
			return new Vector2(worldSize.x, worldSize.y);
		}
		if (meshUvPlane != MeshBoundsUvToShader.MeshUvPlane.YZ)
		{
			return new Vector2(worldSize.x, worldSize.z);
		}
		return new Vector2(worldSize.y, worldSize.z);
	}

	// Token: 0x040026AC RID: 9900
	[SerializeField]
	private Renderer targetRenderer;

	// Token: 0x040026AD RID: 9901
	[SerializeField]
	private MeshFilter targetMeshFilter;

	// Token: 0x040026AE RID: 9902
	[SerializeField]
	private Vector2 meshUvScale = Vector2.one;

	// Token: 0x040026AF RID: 9903
	[SerializeField]
	private Vector2 meshUvOffset = Vector2.zero;

	// Token: 0x040026B0 RID: 9904
	[SerializeField]
	private MeshBoundsUvToShader.MeshUvPlane meshUvPlane;

	// Token: 0x040026B1 RID: 9905
	[SerializeField]
	private bool updateEveryFrame = true;

	// Token: 0x040026B2 RID: 9906
	private readonly int shaderIdIconTexture = Shader.PropertyToID("_IconTexture");

	// Token: 0x040026B3 RID: 9907
	private readonly int shaderIdGridTexture = Shader.PropertyToID("_GridTexture");

	// Token: 0x040026B4 RID: 9908
	private readonly int shaderIdCellsCount = Shader.PropertyToID("_CellsCount");

	// Token: 0x040026B5 RID: 9909
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x040026B6 RID: 9910
	private Vector3 lastScale;

	// Token: 0x040026B7 RID: 9911
	private Vector3 lastPosition;

	// Token: 0x02000724 RID: 1828
	private enum MeshUvPlane
	{
		// Token: 0x040026B9 RID: 9913
		XZ,
		// Token: 0x040026BA RID: 9914
		XY,
		// Token: 0x040026BB RID: 9915
		YZ
	}
}
