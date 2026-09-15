using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Old")]
public class CameraFilterPack_TV_Old : MonoBehaviour
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001D4DE File Offset: 0x0001B6DE
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

	// Token: 0x0600060A RID: 1546 RVA: 0x0001D512 File Offset: 0x0001B712
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Old");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0001D534 File Offset: 0x0001B734
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

	// Token: 0x0600060C RID: 1548 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001D5BA File Offset: 0x0001B7BA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007D3 RID: 2003
	public Shader SCShader;

	// Token: 0x040007D4 RID: 2004
	private float TimeX = 1f;

	// Token: 0x040007D5 RID: 2005
	[Range(1f, 10f)]
	public float Distortion = 1f;

	// Token: 0x040007D6 RID: 2006
	private Material SCMaterial;
}
