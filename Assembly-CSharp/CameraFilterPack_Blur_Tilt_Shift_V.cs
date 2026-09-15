using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Tilt_Shift_V")]
public class CameraFilterPack_Blur_Tilt_Shift_V : MonoBehaviour
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000211 RID: 529 RVA: 0x0000C99C File Offset: 0x0000AB9C
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

	// Token: 0x06000212 RID: 530 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/BlurTiltShift_V");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000C9F4 File Offset: 0x0000ABF4
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
		this.material.SetFloat("_Value1", this.Smooth);
		this.material.SetFloat("_Value2", this.Size);
		this.material.SetFloat("_Value3", this.Position);
		int num = sourceTexture.width / fastFilter;
		int num2 = sourceTexture.height / fastFilter;
		if (this.FastFilter > 1)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(sourceTexture, temporary, this.material, 2);
			Graphics.Blit(temporary, temporary2, this.material, 0);
			this.material.SetFloat("_Amount", this.Amount * 2f);
			Graphics.Blit(temporary2, temporary, this.material, 2);
			Graphics.Blit(temporary, temporary2, this.material, 0);
			this.material.SetTexture("_MainTex2", temporary2);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			Graphics.Blit(sourceTexture, destTexture, this.material, 1);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture, this.material, 0);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000CB84 File Offset: 0x0000AD84
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400032D RID: 813
	public Shader SCShader;

	// Token: 0x0400032E RID: 814
	private float TimeX = 1f;

	// Token: 0x0400032F RID: 815
	private Vector4 ScreenResolution;

	// Token: 0x04000330 RID: 816
	private Material SCMaterial;

	// Token: 0x04000331 RID: 817
	[Range(0f, 20f)]
	public float Amount = 3f;

	// Token: 0x04000332 RID: 818
	[Range(2f, 16f)]
	public int FastFilter = 8;

	// Token: 0x04000333 RID: 819
	[Range(0f, 1f)]
	public float Smooth = 0.5f;

	// Token: 0x04000334 RID: 820
	[Range(0f, 1f)]
	public float Size = 0.5f;

	// Token: 0x04000335 RID: 821
	[Range(-1f, 1f)]
	public float Position = 0.5f;
}
