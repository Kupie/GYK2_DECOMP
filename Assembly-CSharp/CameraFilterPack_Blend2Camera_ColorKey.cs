using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Chroma Key/Color Key")]
public class CameraFilterPack_Blend2Camera_ColorKey : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000DA RID: 218 RVA: 0x0000709D File Offset: 0x0000529D
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

	// Token: 0x060000DB RID: 219 RVA: 0x000070D4 File Offset: 0x000052D4
	private void Start()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture((int)this.ScreenSize.x, (int)this.ScreenSize.y, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00007148 File Offset: 0x00005348
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			if (this.Camera2 != null)
			{
				this.material.SetTexture("_MainTex2", this.Camera2tex);
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
			this.material.SetFloat("_Value2", this.Adjust);
			this.material.SetFloat("_Value3", this.Precision);
			this.material.SetFloat("_Value4", this.Luminosity);
			this.material.SetFloat("_Value5", this.Change_Red);
			this.material.SetFloat("_Value6", this.Change_Green);
			this.material.SetFloat("_Value7", this.Change_Blue);
			this.material.SetColor("_ColorKey", this.ColorKey);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000728F File Offset: 0x0000548F
	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000072B9 File Offset: 0x000054B9
	private void OnEnable()
	{
		this.Start();
		this.Update();
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000072C8 File Offset: 0x000054C8
	private void OnDisable()
	{
		if (this.Camera2 != null && this.Camera2.targetTexture != null)
		{
			this.Camera2.targetTexture = null;
		}
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000193 RID: 403
	private string ShaderName = "CameraFilterPack/Blend2Camera_ColorKey";

	// Token: 0x04000194 RID: 404
	public Shader SCShader;

	// Token: 0x04000195 RID: 405
	public Camera Camera2;

	// Token: 0x04000196 RID: 406
	private float TimeX = 1f;

	// Token: 0x04000197 RID: 407
	private Material SCMaterial;

	// Token: 0x04000198 RID: 408
	[Range(0f, 1f)]
	public float BlendFX = 1f;

	// Token: 0x04000199 RID: 409
	public Color ColorKey;

	// Token: 0x0400019A RID: 410
	[Range(-0.2f, 0.2f)]
	public float Adjust;

	// Token: 0x0400019B RID: 411
	[Range(-0.2f, 0.2f)]
	public float Precision;

	// Token: 0x0400019C RID: 412
	[Range(-0.2f, 0.2f)]
	public float Luminosity;

	// Token: 0x0400019D RID: 413
	[Range(-0.3f, 0.3f)]
	public float Change_Red;

	// Token: 0x0400019E RID: 414
	[Range(-0.3f, 0.3f)]
	public float Change_Green;

	// Token: 0x0400019F RID: 415
	[Range(-0.3f, 0.3f)]
	public float Change_Blue;

	// Token: 0x040001A0 RID: 416
	private RenderTexture Camera2tex;

	// Token: 0x040001A1 RID: 417
	private Vector2 ScreenSize;
}
