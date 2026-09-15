using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/LinearBurn")]
public class CameraFilterPack_Blend2Camera_LinearBurn : MonoBehaviour
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000138 RID: 312 RVA: 0x00008B11 File Offset: 0x00006D11
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

	// Token: 0x06000139 RID: 313 RVA: 0x00008B48 File Offset: 0x00006D48
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

	// Token: 0x0600013A RID: 314 RVA: 0x00008BAC File Offset: 0x00006DAC
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

	// Token: 0x0600013B RID: 315 RVA: 0x00008C9C File Offset: 0x00006E9C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00008C9C File Offset: 0x00006E9C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00008CD4 File Offset: 0x00006ED4
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

	// Token: 0x0400020A RID: 522
	private string ShaderName = "CameraFilterPack/Blend2Camera_LinearBurn";

	// Token: 0x0400020B RID: 523
	public Shader SCShader;

	// Token: 0x0400020C RID: 524
	public Camera Camera2;

	// Token: 0x0400020D RID: 525
	private float TimeX = 1f;

	// Token: 0x0400020E RID: 526
	private Vector4 ScreenResolution;

	// Token: 0x0400020F RID: 527
	private Material SCMaterial;

	// Token: 0x04000210 RID: 528
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000211 RID: 529
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000212 RID: 530
	private RenderTexture Camera2tex;
}
