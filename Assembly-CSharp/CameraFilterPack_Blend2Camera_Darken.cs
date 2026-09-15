using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Darken")]
public class CameraFilterPack_Blend2Camera_Darken : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x00007343 File Offset: 0x00005543
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

	// Token: 0x060000E2 RID: 226 RVA: 0x00007378 File Offset: 0x00005578
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

	// Token: 0x060000E3 RID: 227 RVA: 0x000073DC File Offset: 0x000055DC
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

	// Token: 0x060000E4 RID: 228 RVA: 0x000074CC File Offset: 0x000056CC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000074CC File Offset: 0x000056CC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00007504 File Offset: 0x00005704
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

	// Token: 0x040001A2 RID: 418
	private string ShaderName = "CameraFilterPack/Blend2Camera_Darken";

	// Token: 0x040001A3 RID: 419
	public Shader SCShader;

	// Token: 0x040001A4 RID: 420
	public Camera Camera2;

	// Token: 0x040001A5 RID: 421
	private float TimeX = 1f;

	// Token: 0x040001A6 RID: 422
	private Vector4 ScreenResolution;

	// Token: 0x040001A7 RID: 423
	private Material SCMaterial;

	// Token: 0x040001A8 RID: 424
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001A9 RID: 425
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001AA RID: 426
	private RenderTexture Camera2tex;
}
