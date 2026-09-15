using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Golden")]
public class CameraFilterPack_Edge_Golden : MonoBehaviour
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060003AB RID: 939 RVA: 0x0001332F File Offset: 0x0001152F
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

	// Token: 0x060003AC RID: 940 RVA: 0x00013363 File Offset: 0x00011563
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Golden");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00013384 File Offset: 0x00011584
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
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001341A File Offset: 0x0001161A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000519 RID: 1305
	public Shader SCShader;

	// Token: 0x0400051A RID: 1306
	private float TimeX = 1f;

	// Token: 0x0400051B RID: 1307
	private Vector4 ScreenResolution;

	// Token: 0x0400051C RID: 1308
	private Material SCMaterial;
}
