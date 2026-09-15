using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/HardLight")]
public class CameraFilterPack_Blend2Camera_HardLight : MonoBehaviour
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000110 RID: 272 RVA: 0x00008073 File Offset: 0x00006273
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

	// Token: 0x06000111 RID: 273 RVA: 0x000080A8 File Offset: 0x000062A8
	private void Start()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000810C File Offset: 0x0000630C
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
			this.material.SetFloat("_Value2", this.SwitchCameraToCamera2);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000081FC File Offset: 0x000063FC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000081FC File Offset: 0x000063FC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00008234 File Offset: 0x00006434
	private void OnDisable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2.targetTexture = null;
		}
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040001DD RID: 477
	private string ShaderName = "CameraFilterPack/Blend2Camera_HardLight";

	// Token: 0x040001DE RID: 478
	public Shader SCShader;

	// Token: 0x040001DF RID: 479
	public Camera Camera2;

	// Token: 0x040001E0 RID: 480
	private float TimeX = 1f;

	// Token: 0x040001E1 RID: 481
	private Vector4 ScreenResolution;

	// Token: 0x040001E2 RID: 482
	private Material SCMaterial;

	// Token: 0x040001E3 RID: 483
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001E4 RID: 484
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001E5 RID: 485
	private RenderTexture Camera2tex;
}
