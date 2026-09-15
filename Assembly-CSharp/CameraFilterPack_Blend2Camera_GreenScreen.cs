using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Chroma Key/GreenScreen")]
public class CameraFilterPack_Blend2Camera_GreenScreen : MonoBehaviour
{
	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000109 RID: 265 RVA: 0x00007DE1 File Offset: 0x00005FE1
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

	// Token: 0x0600010A RID: 266 RVA: 0x00007E18 File Offset: 0x00006018
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

	// Token: 0x0600010B RID: 267 RVA: 0x00007E8C File Offset: 0x0000608C
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
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007FBD File Offset: 0x000061BD
	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00007FE7 File Offset: 0x000061E7
	private void OnEnable()
	{
		this.Start();
		this.Update();
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00007FF8 File Offset: 0x000061F8
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

	// Token: 0x040001CF RID: 463
	private string ShaderName = "CameraFilterPack/Blend2Camera_GreenScreen";

	// Token: 0x040001D0 RID: 464
	public Shader SCShader;

	// Token: 0x040001D1 RID: 465
	public Camera Camera2;

	// Token: 0x040001D2 RID: 466
	private float TimeX = 1f;

	// Token: 0x040001D3 RID: 467
	private Material SCMaterial;

	// Token: 0x040001D4 RID: 468
	[Range(0f, 1f)]
	public float BlendFX = 1f;

	// Token: 0x040001D5 RID: 469
	[Range(-0.2f, 0.2f)]
	public float Adjust;

	// Token: 0x040001D6 RID: 470
	[Range(-0.2f, 0.2f)]
	public float Precision;

	// Token: 0x040001D7 RID: 471
	[Range(-0.2f, 0.2f)]
	public float Luminosity;

	// Token: 0x040001D8 RID: 472
	[Range(-0.3f, 0.3f)]
	public float Change_Red;

	// Token: 0x040001D9 RID: 473
	[Range(-0.3f, 0.3f)]
	public float Change_Green;

	// Token: 0x040001DA RID: 474
	[Range(-0.3f, 0.3f)]
	public float Change_Blue;

	// Token: 0x040001DB RID: 475
	private RenderTexture Camera2tex;

	// Token: 0x040001DC RID: 476
	private Vector2 ScreenSize;
}
