using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/WaterDrop")]
public class CameraFilterPack_AAA_WaterDrop : MonoBehaviour
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600007C RID: 124 RVA: 0x000053CC File Offset: 0x000035CC
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

	// Token: 0x0600007D RID: 125 RVA: 0x00005400 File Offset: 0x00003600
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_WaterDrop") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_WaterDrop");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00005438 File Offset: 0x00003638
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
			this.material.SetFloat("_SizeX", this.SizeX);
			this.material.SetFloat("_SizeY", this.SizeY);
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005519 File Offset: 0x00003719
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000106 RID: 262
	public Shader SCShader;

	// Token: 0x04000107 RID: 263
	private float TimeX = 1f;

	// Token: 0x04000108 RID: 264
	[Range(8f, 64f)]
	public float Distortion = 8f;

	// Token: 0x04000109 RID: 265
	[Range(0f, 7f)]
	public float SizeX = 1f;

	// Token: 0x0400010A RID: 266
	[Range(0f, 7f)]
	public float SizeY = 0.5f;

	// Token: 0x0400010B RID: 267
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x0400010C RID: 268
	private Material SCMaterial;

	// Token: 0x0400010D RID: 269
	private Texture2D Texture2;
}
