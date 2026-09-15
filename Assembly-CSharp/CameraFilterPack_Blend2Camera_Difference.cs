using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Difference")]
public class CameraFilterPack_Blend2Camera_Difference : MonoBehaviour
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000F1 RID: 241 RVA: 0x00007781 File Offset: 0x00005981
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

	// Token: 0x060000F2 RID: 242 RVA: 0x000077B8 File Offset: 0x000059B8
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

	// Token: 0x060000F3 RID: 243 RVA: 0x0000781C File Offset: 0x00005A1C
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

	// Token: 0x060000F4 RID: 244 RVA: 0x0000790C File Offset: 0x00005B0C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000790C File Offset: 0x00005B0C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00007944 File Offset: 0x00005B44
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

	// Token: 0x040001B4 RID: 436
	private string ShaderName = "CameraFilterPack/Blend2Camera_Difference";

	// Token: 0x040001B5 RID: 437
	public Shader SCShader;

	// Token: 0x040001B6 RID: 438
	public Camera Camera2;

	// Token: 0x040001B7 RID: 439
	private float TimeX = 1f;

	// Token: 0x040001B8 RID: 440
	private Vector4 ScreenResolution;

	// Token: 0x040001B9 RID: 441
	private Material SCMaterial;

	// Token: 0x040001BA RID: 442
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001BB RID: 443
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001BC RID: 444
	private RenderTexture Camera2tex;
}
