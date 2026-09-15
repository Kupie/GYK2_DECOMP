using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Earth Quake")]
public class CameraFilterPack_FX_EarthQuake : MonoBehaviour
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x000152BD File Offset: 0x000134BD
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

	// Token: 0x0600042B RID: 1067 RVA: 0x000152F1 File Offset: 0x000134F1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_EarthQuake");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00015314 File Offset: 0x00013514
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.X);
			this.material.SetFloat("_Value3", this.Y);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001540C File Offset: 0x0001360C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005AD RID: 1453
	public Shader SCShader;

	// Token: 0x040005AE RID: 1454
	private float TimeX = 1f;

	// Token: 0x040005AF RID: 1455
	private Vector4 ScreenResolution;

	// Token: 0x040005B0 RID: 1456
	private Material SCMaterial;

	// Token: 0x040005B1 RID: 1457
	[Range(0f, 100f)]
	public float Speed = 15f;

	// Token: 0x040005B2 RID: 1458
	[Range(0f, 0.2f)]
	public float X = 0.008f;

	// Token: 0x040005B3 RID: 1459
	[Range(0f, 0.2f)]
	public float Y = 0.008f;

	// Token: 0x040005B4 RID: 1460
	[Range(0f, 0.2f)]
	private float Value4 = 1f;
}
