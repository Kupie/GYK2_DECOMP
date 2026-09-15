using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Color")]
public class CameraFilterPack_Blend2Camera_Color : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060000C2 RID: 194 RVA: 0x00006A40 File Offset: 0x00004C40
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

	// Token: 0x060000C3 RID: 195 RVA: 0x00006A74 File Offset: 0x00004C74
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

	// Token: 0x060000C4 RID: 196 RVA: 0x00006AD8 File Offset: 0x00004CD8
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

	// Token: 0x060000C5 RID: 197 RVA: 0x00006BC8 File Offset: 0x00004DC8
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00006BC8 File Offset: 0x00004DC8
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00006C00 File Offset: 0x00004E00
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

	// Token: 0x04000178 RID: 376
	private string ShaderName = "CameraFilterPack/Blend2Camera_Color";

	// Token: 0x04000179 RID: 377
	public Shader SCShader;

	// Token: 0x0400017A RID: 378
	public Camera Camera2;

	// Token: 0x0400017B RID: 379
	private float TimeX = 1f;

	// Token: 0x0400017C RID: 380
	private Vector4 ScreenResolution;

	// Token: 0x0400017D RID: 381
	private Material SCMaterial;

	// Token: 0x0400017E RID: 382
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400017F RID: 383
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000180 RID: 384
	private RenderTexture Camera2tex;
}
