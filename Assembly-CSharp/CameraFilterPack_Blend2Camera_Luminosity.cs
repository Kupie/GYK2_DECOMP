using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Luminosity")]
public class CameraFilterPack_Blend2Camera_Luminosity : MonoBehaviour
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000150 RID: 336 RVA: 0x00009171 File Offset: 0x00007371
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

	// Token: 0x06000151 RID: 337 RVA: 0x000091A8 File Offset: 0x000073A8
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

	// Token: 0x06000152 RID: 338 RVA: 0x0000920C File Offset: 0x0000740C
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

	// Token: 0x06000153 RID: 339 RVA: 0x000092FC File Offset: 0x000074FC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000155 RID: 341 RVA: 0x000092FC File Offset: 0x000074FC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009334 File Offset: 0x00007534
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

	// Token: 0x04000225 RID: 549
	private string ShaderName = "CameraFilterPack/Blend2Camera_Luminosity";

	// Token: 0x04000226 RID: 550
	public Shader SCShader;

	// Token: 0x04000227 RID: 551
	public Camera Camera2;

	// Token: 0x04000228 RID: 552
	private float TimeX = 1f;

	// Token: 0x04000229 RID: 553
	private Vector4 ScreenResolution;

	// Token: 0x0400022A RID: 554
	private Material SCMaterial;

	// Token: 0x0400022B RID: 555
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400022C RID: 556
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400022D RID: 557
	private RenderTexture Camera2tex;
}
