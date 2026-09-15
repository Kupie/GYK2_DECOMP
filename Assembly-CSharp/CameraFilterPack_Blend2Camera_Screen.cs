using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Screen")]
public class CameraFilterPack_Blend2Camera_Screen : MonoBehaviour
{
	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000181 RID: 385 RVA: 0x0000A0FD File Offset: 0x000082FD
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

	// Token: 0x06000182 RID: 386 RVA: 0x0000A134 File Offset: 0x00008334
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

	// Token: 0x06000183 RID: 387 RVA: 0x0000A198 File Offset: 0x00008398
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

	// Token: 0x06000184 RID: 388 RVA: 0x0000A288 File Offset: 0x00008488
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A288 File Offset: 0x00008488
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A2C0 File Offset: 0x000084C0
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

	// Token: 0x04000277 RID: 631
	private string ShaderName = "CameraFilterPack/Blend2Camera_Screen";

	// Token: 0x04000278 RID: 632
	public Shader SCShader;

	// Token: 0x04000279 RID: 633
	public Camera Camera2;

	// Token: 0x0400027A RID: 634
	private float TimeX = 1f;

	// Token: 0x0400027B RID: 635
	private Vector4 ScreenResolution;

	// Token: 0x0400027C RID: 636
	private Material SCMaterial;

	// Token: 0x0400027D RID: 637
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400027E RID: 638
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400027F RID: 639
	private RenderTexture Camera2tex;
}
