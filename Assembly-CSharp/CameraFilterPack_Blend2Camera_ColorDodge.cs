using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/ColorDodge")]
public class CameraFilterPack_Blend2Camera_ColorDodge : MonoBehaviour
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006E7D File Offset: 0x0000507D
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

	// Token: 0x060000D3 RID: 211 RVA: 0x00006EB4 File Offset: 0x000050B4
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

	// Token: 0x060000D4 RID: 212 RVA: 0x00006F18 File Offset: 0x00005118
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

	// Token: 0x060000D5 RID: 213 RVA: 0x00007008 File Offset: 0x00005208
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00007008 File Offset: 0x00005208
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00007040 File Offset: 0x00005240
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

	// Token: 0x0400018A RID: 394
	private string ShaderName = "CameraFilterPack/Blend2Camera_ColorDodge";

	// Token: 0x0400018B RID: 395
	public Shader SCShader;

	// Token: 0x0400018C RID: 396
	public Camera Camera2;

	// Token: 0x0400018D RID: 397
	private float TimeX = 1f;

	// Token: 0x0400018E RID: 398
	private Vector4 ScreenResolution;

	// Token: 0x0400018F RID: 399
	private Material SCMaterial;

	// Token: 0x04000190 RID: 400
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000191 RID: 401
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000192 RID: 402
	private RenderTexture Camera2tex;
}
