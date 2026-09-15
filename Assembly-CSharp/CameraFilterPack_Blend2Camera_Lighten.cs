using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Lighten")]
public class CameraFilterPack_Blend2Camera_Lighten : MonoBehaviour
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000128 RID: 296 RVA: 0x000086D1 File Offset: 0x000068D1
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

	// Token: 0x06000129 RID: 297 RVA: 0x00008708 File Offset: 0x00006908
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

	// Token: 0x0600012A RID: 298 RVA: 0x0000876C File Offset: 0x0000696C
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

	// Token: 0x0600012B RID: 299 RVA: 0x0000885C File Offset: 0x00006A5C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000885C File Offset: 0x00006A5C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008894 File Offset: 0x00006A94
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

	// Token: 0x040001F8 RID: 504
	private string ShaderName = "CameraFilterPack/Blend2Camera_Lighten";

	// Token: 0x040001F9 RID: 505
	public Shader SCShader;

	// Token: 0x040001FA RID: 506
	public Camera Camera2;

	// Token: 0x040001FB RID: 507
	private float TimeX = 1f;

	// Token: 0x040001FC RID: 508
	private Vector4 ScreenResolution;

	// Token: 0x040001FD RID: 509
	private Material SCMaterial;

	// Token: 0x040001FE RID: 510
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001FF RID: 511
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000200 RID: 512
	private RenderTexture Camera2tex;
}
