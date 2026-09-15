using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/VividLight")]
public class CameraFilterPack_Blend2Camera_VividLight : MonoBehaviour
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000AD11 File Offset: 0x00008F11
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

	// Token: 0x060001AA RID: 426 RVA: 0x0000AD48 File Offset: 0x00008F48
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

	// Token: 0x060001AB RID: 427 RVA: 0x0000ADAC File Offset: 0x00008FAC
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

	// Token: 0x060001AC RID: 428 RVA: 0x0000AE9C File Offset: 0x0000909C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000AE9C File Offset: 0x0000909C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000AED4 File Offset: 0x000090D4
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

	// Token: 0x040002B1 RID: 689
	private string ShaderName = "CameraFilterPack/Blend2Camera_VividLight";

	// Token: 0x040002B2 RID: 690
	public Shader SCShader;

	// Token: 0x040002B3 RID: 691
	public Camera Camera2;

	// Token: 0x040002B4 RID: 692
	private float TimeX = 1f;

	// Token: 0x040002B5 RID: 693
	private Vector4 ScreenResolution;

	// Token: 0x040002B6 RID: 694
	private Material SCMaterial;

	// Token: 0x040002B7 RID: 695
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040002B8 RID: 696
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040002B9 RID: 697
	private RenderTexture Camera2tex;
}
