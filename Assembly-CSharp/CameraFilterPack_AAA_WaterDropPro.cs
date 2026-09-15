using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/WaterDropPro")]
public class CameraFilterPack_AAA_WaterDropPro : MonoBehaviour
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000082 RID: 130 RVA: 0x00005572 File Offset: 0x00003772
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

	// Token: 0x06000083 RID: 131 RVA: 0x000055A6 File Offset: 0x000037A6
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_WaterDrop") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_WaterDropPro");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000055DC File Offset: 0x000037DC
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

	// Token: 0x06000085 RID: 133 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000056BD File Offset: 0x000038BD
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400010E RID: 270
	public Shader SCShader;

	// Token: 0x0400010F RID: 271
	private float TimeX = 1f;

	// Token: 0x04000110 RID: 272
	[Range(8f, 64f)]
	public float Distortion = 8f;

	// Token: 0x04000111 RID: 273
	[Range(0f, 7f)]
	public float SizeX = 1f;

	// Token: 0x04000112 RID: 274
	[Range(0f, 7f)]
	public float SizeY = 0.5f;

	// Token: 0x04000113 RID: 275
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x04000114 RID: 276
	private Material SCMaterial;

	// Token: 0x04000115 RID: 277
	private Texture2D Texture2;
}
