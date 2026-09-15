using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/PhotoshopFilters")]
public class CameraFilterPack_Blend2Camera_PhotoshopFilters : MonoBehaviour
{
	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000168 RID: 360 RVA: 0x000097D1 File Offset: 0x000079D1
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

	// Token: 0x06000169 RID: 361 RVA: 0x00009808 File Offset: 0x00007A08
	private void ChangeFilters()
	{
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Darken)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Darken";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Multiply)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Multiply";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorBurn)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_ColorBurn";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearBurn)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearBurn";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.DarkerColor)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_DarkerColor";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Lighten)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Lighten";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Screen)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Screen";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorDodge)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_ColorDodge";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearDodge)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearDodge";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LighterColor)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LighterColor";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Overlay)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Overlay";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.SoftLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_SoftLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_HardLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.VividLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_VividLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.PinLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_PinLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardMix)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_HardMix";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Difference)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Difference";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Exclusion)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Exclusion";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Subtract)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Subtract";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Divide)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Divide";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Hue)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Hue";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Saturation)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Saturation";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Color)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Color";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Luminosity)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Luminosity";
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00009A18 File Offset: 0x00007C18
	private void Start()
	{
		this.filterchoicememo = this.filterchoice;
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		this.ChangeFilters();
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00009A90 File Offset: 0x00007C90
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

	// Token: 0x0600016C RID: 364 RVA: 0x00009B80 File Offset: 0x00007D80
	private void OnValidate()
	{
		if (this.filterchoice != this.filterchoicememo)
		{
			this.ChangeFilters();
			this.SCShader = Shader.Find(this.ShaderName);
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
		}
		this.filterchoicememo = this.filterchoice;
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009C2B File Offset: 0x00007E2B
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00009C63 File Offset: 0x00007E63
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

	// Token: 0x04000240 RID: 576
	private string ShaderName = "CameraFilterPack/Blend2Camera_Darken";

	// Token: 0x04000241 RID: 577
	public Shader SCShader;

	// Token: 0x04000242 RID: 578
	public Camera Camera2;

	// Token: 0x04000243 RID: 579
	public CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoice;

	// Token: 0x04000244 RID: 580
	private CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoicememo;

	// Token: 0x04000245 RID: 581
	private float TimeX = 1f;

	// Token: 0x04000246 RID: 582
	private Vector4 ScreenResolution;

	// Token: 0x04000247 RID: 583
	private Material SCMaterial;

	// Token: 0x04000248 RID: 584
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000249 RID: 585
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400024A RID: 586
	private RenderTexture Camera2tex;

	// Token: 0x02000038 RID: 56
	public enum filters
	{
		// Token: 0x0400024C RID: 588
		Darken,
		// Token: 0x0400024D RID: 589
		Multiply,
		// Token: 0x0400024E RID: 590
		ColorBurn,
		// Token: 0x0400024F RID: 591
		LinearBurn,
		// Token: 0x04000250 RID: 592
		DarkerColor,
		// Token: 0x04000251 RID: 593
		Lighten,
		// Token: 0x04000252 RID: 594
		Screen,
		// Token: 0x04000253 RID: 595
		ColorDodge,
		// Token: 0x04000254 RID: 596
		LinearDodge,
		// Token: 0x04000255 RID: 597
		LighterColor,
		// Token: 0x04000256 RID: 598
		Overlay,
		// Token: 0x04000257 RID: 599
		SoftLight,
		// Token: 0x04000258 RID: 600
		HardLight,
		// Token: 0x04000259 RID: 601
		VividLight,
		// Token: 0x0400025A RID: 602
		LinearLight,
		// Token: 0x0400025B RID: 603
		PinLight,
		// Token: 0x0400025C RID: 604
		HardMix,
		// Token: 0x0400025D RID: 605
		Difference,
		// Token: 0x0400025E RID: 606
		Exclusion,
		// Token: 0x0400025F RID: 607
		Subtract,
		// Token: 0x04000260 RID: 608
		Divide,
		// Token: 0x04000261 RID: 609
		Hue,
		// Token: 0x04000262 RID: 610
		Saturation,
		// Token: 0x04000263 RID: 611
		Color,
		// Token: 0x04000264 RID: 612
		Luminosity
	}
}
