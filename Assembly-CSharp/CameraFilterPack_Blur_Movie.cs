using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Movie")]
public class CameraFilterPack_Blur_Movie : MonoBehaviour
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000BB6B File Offset: 0x00009D6B
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

	// Token: 0x060001E2 RID: 482 RVA: 0x0000BB9F File Offset: 0x00009D9F
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Movie");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000BBC0 File Offset: 0x00009DC0
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
		this.material.SetFloat("_Radius", this.Radius / (float)fastFilter);
		this.material.SetFloat("_Factor", this.Factor);
		this.material.SetVector("_ScreenResolution", new Vector2((float)(Screen.width / fastFilter), (float)(Screen.height / fastFilter)));
		int num = sourceTexture.width / fastFilter;
		int num2 = sourceTexture.height / fastFilter;
		if (this.FastFilter > 1)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
			Graphics.Blit(sourceTexture, temporary, this.material);
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture, this.material);
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000BCD2 File Offset: 0x00009ED2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002F1 RID: 753
	public Shader SCShader;

	// Token: 0x040002F2 RID: 754
	private float TimeX = 1f;

	// Token: 0x040002F3 RID: 755
	private Vector4 ScreenResolution;

	// Token: 0x040002F4 RID: 756
	private Material SCMaterial;

	// Token: 0x040002F5 RID: 757
	[Range(0f, 1000f)]
	public float Radius = 150f;

	// Token: 0x040002F6 RID: 758
	[Range(0f, 1000f)]
	public float Factor = 200f;

	// Token: 0x040002F7 RID: 759
	[Range(1f, 8f)]
	public int FastFilter = 2;
}
