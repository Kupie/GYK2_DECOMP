using System;
using UnityEngine;

// Token: 0x020000FF RID: 255
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/VCR Distortion")]
public class CameraFilterPack_TV_Vcr : MonoBehaviour
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001DE05 File Offset: 0x0001C005
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

	// Token: 0x06000634 RID: 1588 RVA: 0x0001DE39 File Offset: 0x0001C039
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Vcr");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001DE5C File Offset: 0x0001C05C
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

	// Token: 0x06000636 RID: 1590 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0001DEE2 File Offset: 0x0001C0E2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007FA RID: 2042
	public Shader SCShader;

	// Token: 0x040007FB RID: 2043
	private float TimeX = 1f;

	// Token: 0x040007FC RID: 2044
	[Range(1f, 10f)]
	public float Distortion = 1f;

	// Token: 0x040007FD RID: 2045
	private Material SCMaterial;
}
