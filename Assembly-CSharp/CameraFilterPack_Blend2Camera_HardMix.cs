using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/HardMix")]
public class CameraFilterPack_Blend2Camera_HardMix : MonoBehaviour
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000118 RID: 280 RVA: 0x00008291 File Offset: 0x00006491
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

	// Token: 0x06000119 RID: 281 RVA: 0x000082C8 File Offset: 0x000064C8
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

	// Token: 0x0600011A RID: 282 RVA: 0x0000832C File Offset: 0x0000652C
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

	// Token: 0x0600011B RID: 283 RVA: 0x0000841C File Offset: 0x0000661C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000841C File Offset: 0x0000661C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008454 File Offset: 0x00006654
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

	// Token: 0x040001E6 RID: 486
	private string ShaderName = "CameraFilterPack/Blend2Camera_HardMix";

	// Token: 0x040001E7 RID: 487
	public Shader SCShader;

	// Token: 0x040001E8 RID: 488
	public Camera Camera2;

	// Token: 0x040001E9 RID: 489
	private float TimeX = 1f;

	// Token: 0x040001EA RID: 490
	private Vector4 ScreenResolution;

	// Token: 0x040001EB RID: 491
	private Material SCMaterial;

	// Token: 0x040001EC RID: 492
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001ED RID: 493
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001EE RID: 494
	private RenderTexture Camera2tex;
}
