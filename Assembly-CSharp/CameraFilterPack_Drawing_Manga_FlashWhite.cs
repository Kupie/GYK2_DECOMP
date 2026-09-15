using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga_FlashWhite")]
public class CameraFilterPack_Drawing_Manga_FlashWhite : MonoBehaviour
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000375 RID: 885 RVA: 0x00012301 File Offset: 0x00010501
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

	// Token: 0x06000376 RID: 886 RVA: 0x00012335 File Offset: 0x00010535
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga_FlashWhite");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00012358 File Offset: 0x00010558
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", (float)this.Speed);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Intensity", this.Intensity);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00012467 File Offset: 0x00010667
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004CA RID: 1226
	public Shader SCShader;

	// Token: 0x040004CB RID: 1227
	private float TimeX = 1f;

	// Token: 0x040004CC RID: 1228
	private Vector4 ScreenResolution;

	// Token: 0x040004CD RID: 1229
	private Material SCMaterial;

	// Token: 0x040004CE RID: 1230
	[Range(1f, 10f)]
	public float Size = 1f;

	// Token: 0x040004CF RID: 1231
	[Range(0f, 30f)]
	public int Speed = 5;

	// Token: 0x040004D0 RID: 1232
	[Range(-1f, 1f)]
	public float PosX = 0.5f;

	// Token: 0x040004D1 RID: 1233
	[Range(-1f, 1f)]
	public float PosY = 0.5f;

	// Token: 0x040004D2 RID: 1234
	[Range(0f, 1f)]
	public float Intensity = 1f;
}
