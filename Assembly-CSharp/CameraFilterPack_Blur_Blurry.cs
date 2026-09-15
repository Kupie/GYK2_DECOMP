using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Blurry")]
public class CameraFilterPack_Blur_Blurry : MonoBehaviour
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000B41C File Offset: 0x0000961C
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

	// Token: 0x060001C4 RID: 452 RVA: 0x0000B450 File Offset: 0x00009650
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Blurry");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000B474 File Offset: 0x00009674
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (!(this.SCShader != null))
		{
			Graphics.Blit(sourceTexture, destTexture);
			return;
		}
		int fastFilter = this.FastFilter;
		this.TimeX += Time.deltaTime;
		if (this.TimeX > 100f)
		{
			this.TimeX = 0f;
		}
		this.material.SetFloat("_TimeX", this.TimeX);
		this.material.SetFloat("_Amount", this.Amount);
		this.material.SetVector("_ScreenResolution", new Vector2((float)(Screen.width / fastFilter), (float)(Screen.height / fastFilter)));
		int num = sourceTexture.width / fastFilter;
		int num2 = sourceTexture.height / fastFilter;
		if (this.FastFilter > 1)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(sourceTexture, temporary, this.material);
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture, this.material);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000B574 File Offset: 0x00009774
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002D2 RID: 722
	public Shader SCShader;

	// Token: 0x040002D3 RID: 723
	private float TimeX = 1f;

	// Token: 0x040002D4 RID: 724
	private Vector4 ScreenResolution;

	// Token: 0x040002D5 RID: 725
	private Material SCMaterial;

	// Token: 0x040002D6 RID: 726
	[Range(0f, 20f)]
	public float Amount = 2f;

	// Token: 0x040002D7 RID: 727
	[Range(1f, 8f)]
	public int FastFilter = 2;
}
