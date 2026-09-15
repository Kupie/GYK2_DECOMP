using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/LinearLight")]
public class CameraFilterPack_Blend2Camera_LinearLight : MonoBehaviour
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000148 RID: 328 RVA: 0x00008F51 File Offset: 0x00007151
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

	// Token: 0x06000149 RID: 329 RVA: 0x00008F88 File Offset: 0x00007188
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

	// Token: 0x0600014A RID: 330 RVA: 0x00008FEC File Offset: 0x000071EC
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

	// Token: 0x0600014B RID: 331 RVA: 0x000090DC File Offset: 0x000072DC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000090DC File Offset: 0x000072DC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00009114 File Offset: 0x00007314
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

	// Token: 0x0400021C RID: 540
	private string ShaderName = "CameraFilterPack/Blend2Camera_LinearLight";

	// Token: 0x0400021D RID: 541
	public Shader SCShader;

	// Token: 0x0400021E RID: 542
	public Camera Camera2;

	// Token: 0x0400021F RID: 543
	private float TimeX = 1f;

	// Token: 0x04000220 RID: 544
	private Vector4 ScreenResolution;

	// Token: 0x04000221 RID: 545
	private Material SCMaterial;

	// Token: 0x04000222 RID: 546
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000223 RID: 547
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000224 RID: 548
	private RenderTexture Camera2tex;
}
