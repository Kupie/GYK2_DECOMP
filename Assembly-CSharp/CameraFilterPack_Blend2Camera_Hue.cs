using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Hue")]
public class CameraFilterPack_Blend2Camera_Hue : MonoBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000120 RID: 288 RVA: 0x000084B1 File Offset: 0x000066B1
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

	// Token: 0x06000121 RID: 289 RVA: 0x000084E8 File Offset: 0x000066E8
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

	// Token: 0x06000122 RID: 290 RVA: 0x0000854C File Offset: 0x0000674C
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

	// Token: 0x06000123 RID: 291 RVA: 0x0000863C File Offset: 0x0000683C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000863C File Offset: 0x0000683C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00008674 File Offset: 0x00006874
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

	// Token: 0x040001EF RID: 495
	private string ShaderName = "CameraFilterPack/Blend2Camera_Hue";

	// Token: 0x040001F0 RID: 496
	public Shader SCShader;

	// Token: 0x040001F1 RID: 497
	public Camera Camera2;

	// Token: 0x040001F2 RID: 498
	private float TimeX = 1f;

	// Token: 0x040001F3 RID: 499
	private Vector4 ScreenResolution;

	// Token: 0x040001F4 RID: 500
	private Material SCMaterial;

	// Token: 0x040001F5 RID: 501
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001F6 RID: 502
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001F7 RID: 503
	private RenderTexture Camera2tex;
}
