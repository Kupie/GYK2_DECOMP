using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/8bits")]
public class CameraFilterPack_FX_8bits : MonoBehaviour
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001441B File Offset: 0x0001261B
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

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001444F File Offset: 0x0001264F
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_8bits");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00014470 File Offset: 0x00012670
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
			if (this.Brightness == 0f)
			{
				this.Brightness = 0.001f;
			}
			this.material.SetFloat("_Distortion", this.Brightness);
			RenderTexture temporary = RenderTexture.GetTemporary(this.ResolutionX, this.ResolutionY, 0);
			Graphics.Blit(sourceTexture, temporary, this.material);
			temporary.filterMode = FilterMode.Point;
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00014538 File Offset: 0x00012738
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000567 RID: 1383
	public Shader SCShader;

	// Token: 0x04000568 RID: 1384
	private float TimeX = 1f;

	// Token: 0x04000569 RID: 1385
	private Material SCMaterial;

	// Token: 0x0400056A RID: 1386
	[Range(-1f, 1f)]
	public float Brightness;

	// Token: 0x0400056B RID: 1387
	[Range(80f, 640f)]
	public int ResolutionX = 160;

	// Token: 0x0400056C RID: 1388
	[Range(60f, 480f)]
	public int ResolutionY = 240;
}
