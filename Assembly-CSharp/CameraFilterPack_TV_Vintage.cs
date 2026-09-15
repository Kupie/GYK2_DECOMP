using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Vintage")]
public class CameraFilterPack_TV_Vintage : MonoBehaviour
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001E5F2 File Offset: 0x0001C7F2
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0001E626 File Offset: 0x0001C826
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Vintage");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0001E648 File Offset: 0x0001C848
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Distortion", this.Distortion);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0001E6CE File Offset: 0x0001C8CE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400081D RID: 2077
	public Shader SCShader;

	// Token: 0x0400081E RID: 2078
	private float TimeX = 1f;

	// Token: 0x0400081F RID: 2079
	[Range(1f, 10f)]
	public float Distortion = 1f;

	// Token: 0x04000820 RID: 2080
	private Material SCMaterial;
}
