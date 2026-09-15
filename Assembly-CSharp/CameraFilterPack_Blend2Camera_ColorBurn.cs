using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/ColorBurn")]
public class CameraFilterPack_Blend2Camera_ColorBurn : MonoBehaviour
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000CA RID: 202 RVA: 0x00006C5D File Offset: 0x00004E5D
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

	// Token: 0x060000CB RID: 203 RVA: 0x00006C94 File Offset: 0x00004E94
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

	// Token: 0x060000CC RID: 204 RVA: 0x00006CF8 File Offset: 0x00004EF8
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

	// Token: 0x060000CD RID: 205 RVA: 0x00006DE8 File Offset: 0x00004FE8
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00006DE8 File Offset: 0x00004FE8
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00006E20 File Offset: 0x00005020
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

	// Token: 0x04000181 RID: 385
	private string ShaderName = "CameraFilterPack/Blend2Camera_ColorBurn";

	// Token: 0x04000182 RID: 386
	public Shader SCShader;

	// Token: 0x04000183 RID: 387
	public Camera Camera2;

	// Token: 0x04000184 RID: 388
	private float TimeX = 1f;

	// Token: 0x04000185 RID: 389
	private Vector4 ScreenResolution;

	// Token: 0x04000186 RID: 390
	private Material SCMaterial;

	// Token: 0x04000187 RID: 391
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000188 RID: 392
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000189 RID: 393
	private RenderTexture Camera2tex;
}
