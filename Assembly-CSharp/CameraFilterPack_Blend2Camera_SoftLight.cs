using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/SoftLight")]
public class CameraFilterPack_Blend2Camera_SoftLight : MonoBehaviour
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000189 RID: 393 RVA: 0x0000A31D File Offset: 0x0000851D
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

	// Token: 0x0600018A RID: 394 RVA: 0x0000A354 File Offset: 0x00008554
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

	// Token: 0x0600018B RID: 395 RVA: 0x0000A3B8 File Offset: 0x000085B8
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

	// Token: 0x0600018C RID: 396 RVA: 0x0000A4A8 File Offset: 0x000086A8
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000A4A8 File Offset: 0x000086A8
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000A4E0 File Offset: 0x000086E0
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

	// Token: 0x04000280 RID: 640
	private string ShaderName = "CameraFilterPack/Blend2Camera_SoftLight";

	// Token: 0x04000281 RID: 641
	public Shader SCShader;

	// Token: 0x04000282 RID: 642
	public Camera Camera2;

	// Token: 0x04000283 RID: 643
	private float TimeX = 1f;

	// Token: 0x04000284 RID: 644
	private Vector4 ScreenResolution;

	// Token: 0x04000285 RID: 645
	private Material SCMaterial;

	// Token: 0x04000286 RID: 646
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000287 RID: 647
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000288 RID: 648
	private RenderTexture Camera2tex;
}
