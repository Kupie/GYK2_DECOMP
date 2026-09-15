using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Exclusion")]
public class CameraFilterPack_Blend2Camera_Exclusion : MonoBehaviour
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000101 RID: 257 RVA: 0x00007BC1 File Offset: 0x00005DC1
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

	// Token: 0x06000102 RID: 258 RVA: 0x00007BF8 File Offset: 0x00005DF8
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

	// Token: 0x06000103 RID: 259 RVA: 0x00007C5C File Offset: 0x00005E5C
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

	// Token: 0x06000104 RID: 260 RVA: 0x00007D4C File Offset: 0x00005F4C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00007D4C File Offset: 0x00005F4C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00007D84 File Offset: 0x00005F84
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

	// Token: 0x040001C6 RID: 454
	private string ShaderName = "CameraFilterPack/Blend2Camera_Exclusion";

	// Token: 0x040001C7 RID: 455
	public Shader SCShader;

	// Token: 0x040001C8 RID: 456
	public Camera Camera2;

	// Token: 0x040001C9 RID: 457
	private float TimeX = 1f;

	// Token: 0x040001CA RID: 458
	private Vector4 ScreenResolution;

	// Token: 0x040001CB RID: 459
	private Material SCMaterial;

	// Token: 0x040001CC RID: 460
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001CD RID: 461
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001CE RID: 462
	private RenderTexture Camera2tex;
}
