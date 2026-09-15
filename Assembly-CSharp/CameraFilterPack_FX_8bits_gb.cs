using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/8bits_gb")]
public class CameraFilterPack_FX_8bits_gb : MonoBehaviour
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001457B File Offset: 0x0001277B
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

	// Token: 0x060003FB RID: 1019 RVA: 0x000145AF File Offset: 0x000127AF
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_8bits_gb");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x000145D0 File Offset: 0x000127D0
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
			RenderTexture temporary = RenderTexture.GetTemporary(160, 144, 0);
			Graphics.Blit(sourceTexture, temporary, this.material);
			temporary.filterMode = FilterMode.Point;
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00014696 File Offset: 0x00012896
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400056D RID: 1389
	public Shader SCShader;

	// Token: 0x0400056E RID: 1390
	private float TimeX = 1f;

	// Token: 0x0400056F RID: 1391
	private Material SCMaterial;

	// Token: 0x04000570 RID: 1392
	[Range(-1f, 1f)]
	public float Brightness;
}
