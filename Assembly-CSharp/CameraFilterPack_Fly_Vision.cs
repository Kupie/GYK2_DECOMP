using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Fly_Vision")]
public class CameraFilterPack_Fly_Vision : MonoBehaviour
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001424A File Offset: 0x0001244A
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

	// Token: 0x060003EF RID: 1007 RVA: 0x0001427E File Offset: 0x0001247E
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Fly_VisionFX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Fly_Vision");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x000142B4 File Offset: 0x000124B4
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
			this.material.SetFloat("_Value", this.Zoom);
			this.material.SetFloat("_Value2", this.Distortion);
			this.material.SetFloat("_Value3", this.Fade);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x000143C2 File Offset: 0x000125C2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400055E RID: 1374
	public Shader SCShader;

	// Token: 0x0400055F RID: 1375
	private float TimeX = 1f;

	// Token: 0x04000560 RID: 1376
	private Vector4 ScreenResolution;

	// Token: 0x04000561 RID: 1377
	private Material SCMaterial;

	// Token: 0x04000562 RID: 1378
	[Range(0.04f, 1.5f)]
	public float Zoom = 0.25f;

	// Token: 0x04000563 RID: 1379
	[Range(0f, 1f)]
	public float Distortion = 0.4f;

	// Token: 0x04000564 RID: 1380
	[Range(0f, 1f)]
	public float Fade = 0.4f;

	// Token: 0x04000565 RID: 1381
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x04000566 RID: 1382
	private Texture2D Texture2;
}
