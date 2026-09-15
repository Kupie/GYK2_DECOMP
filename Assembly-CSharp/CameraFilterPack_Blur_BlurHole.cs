using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Blur Hole")]
public class CameraFilterPack_Blur_BlurHole : MonoBehaviour
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B214 File Offset: 0x00009414
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

	// Token: 0x060001BE RID: 446 RVA: 0x0000B248 File Offset: 0x00009448
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/BlurHole");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000B26C File Offset: 0x0000946C
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Distortion", this.Size);
			this.material.SetFloat("_Radius", this._Radius);
			this.material.SetFloat("_SpotSize", this._SpotSize);
			this.material.SetFloat("_CenterX", this._CenterX);
			this.material.SetFloat("_CenterY", this._CenterY);
			this.material.SetFloat("_Alpha", this._AlphaBlur);
			this.material.SetFloat("_Alpha2", this._AlphaBlurInside);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000B39F File Offset: 0x0000959F
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002C7 RID: 711
	public Shader SCShader;

	// Token: 0x040002C8 RID: 712
	private float TimeX = 1f;

	// Token: 0x040002C9 RID: 713
	[Range(1f, 16f)]
	public float Size = 10f;

	// Token: 0x040002CA RID: 714
	[Range(0f, 1f)]
	public float _Radius = 0.25f;

	// Token: 0x040002CB RID: 715
	[Range(0f, 4f)]
	public float _SpotSize = 1f;

	// Token: 0x040002CC RID: 716
	[Range(0f, 1f)]
	public float _CenterX = 0.5f;

	// Token: 0x040002CD RID: 717
	[Range(0f, 1f)]
	public float _CenterY = 0.5f;

	// Token: 0x040002CE RID: 718
	[Range(0f, 1f)]
	public float _AlphaBlur = 1f;

	// Token: 0x040002CF RID: 719
	[Range(0f, 1f)]
	public float _AlphaBlurInside;

	// Token: 0x040002D0 RID: 720
	private Vector4 ScreenResolution;

	// Token: 0x040002D1 RID: 721
	private Material SCMaterial;
}
