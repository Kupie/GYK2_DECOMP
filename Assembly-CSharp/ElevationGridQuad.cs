using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000154 RID: 340
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ElevationGridQuad : MonoBehaviour
{
	// Token: 0x0600080B RID: 2059 RVA: 0x00027700 File Offset: 0x00025900
	public void Setup(Material gridMaterial)
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		component.sharedMesh = ElevationGridQuad.GetSharedMesh();
		this.materialInstance = new Material(gridMaterial);
		this.meshRenderer.material = this.materialInstance;
		this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		this.meshRenderer.receiveShadows = false;
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00027760 File Offset: 0x00025960
	public void Draw(Color[] data, Color[] selection, int size, Vector3 worldCenter, Vector2 worldSize, int buildMode)
	{
		this.EnsureTextures(size);
		this.dataTexture.SetPixels(data);
		this.dataTexture.Apply();
		this.selectionTexture.SetPixels(selection);
		this.selectionTexture.Apply();
		Transform parent = base.transform.parent;
		Vector3 vector = ((parent != null) ? parent.lossyScale : Vector3.one);
		base.transform.localScale = new Vector3(worldSize.x / vector.x, 1f / vector.y, worldSize.y / vector.z);
		base.transform.position = worldCenter;
		this.materialInstance.SetTexture(ElevationGridQuad.dataTexId, this.dataTexture);
		this.materialInstance.SetTexture(ElevationGridQuad.selectionTexId, this.selectionTexture);
		this.materialInstance.SetInt(ElevationGridQuad.buildModeId, buildMode);
		this.materialInstance.SetVector(ElevationGridQuad.objectScaleId, base.transform.lossyScale);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00027884 File Offset: 0x00025A84
	private void EnsureTextures(int size)
	{
		if (this.dataTexture != null && this.textureSize == size)
		{
			return;
		}
		if (this.dataTexture != null)
		{
			global::UnityEngine.Object.Destroy(this.dataTexture);
		}
		if (this.selectionTexture != null)
		{
			global::UnityEngine.Object.Destroy(this.selectionTexture);
		}
		this.dataTexture = new Texture2D(size, size, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		this.selectionTexture = new Texture2D(size, size, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		this.textureSize = size;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0002791C File Offset: 0x00025B1C
	private void OnDestroy()
	{
		if (this.dataTexture != null)
		{
			global::UnityEngine.Object.Destroy(this.dataTexture);
		}
		if (this.selectionTexture != null)
		{
			global::UnityEngine.Object.Destroy(this.selectionTexture);
		}
		if (this.materialInstance != null)
		{
			global::UnityEngine.Object.Destroy(this.materialInstance);
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00027974 File Offset: 0x00025B74
	private static Mesh GetSharedMesh()
	{
		if (ElevationGridQuad.sharedMesh != null)
		{
			return ElevationGridQuad.sharedMesh;
		}
		ElevationGridQuad.sharedMesh = new Mesh
		{
			name = "ElevationGridQuad"
		};
		ElevationGridQuad.sharedMesh.vertices = new Vector3[]
		{
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f)
		};
		ElevationGridQuad.sharedMesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		ElevationGridQuad.sharedMesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
		ElevationGridQuad.sharedMesh.normals = new Vector3[]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
		ElevationGridQuad.sharedMesh.RecalculateBounds();
		return ElevationGridQuad.sharedMesh;
	}

	// Token: 0x04000A02 RID: 2562
	private static readonly int dataTexId = Shader.PropertyToID("_DataTex");

	// Token: 0x04000A03 RID: 2563
	private static readonly int selectionTexId = Shader.PropertyToID("_SelectionTex");

	// Token: 0x04000A04 RID: 2564
	private static readonly int objectScaleId = Shader.PropertyToID("_ObjectScale");

	// Token: 0x04000A05 RID: 2565
	private static readonly int buildModeId = Shader.PropertyToID("_BuildMode");

	// Token: 0x04000A06 RID: 2566
	private static Mesh sharedMesh;

	// Token: 0x04000A07 RID: 2567
	private MeshRenderer meshRenderer;

	// Token: 0x04000A08 RID: 2568
	private Material materialInstance;

	// Token: 0x04000A09 RID: 2569
	private Texture2D dataTexture;

	// Token: 0x04000A0A RID: 2570
	private Texture2D selectionTexture;

	// Token: 0x04000A0B RID: 2571
	private int textureSize;
}
