using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Tilt_Shift_Hole")]
public class CameraFilterPack_Blur_Tilt_Shift_Hole : MonoBehaviour
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600020B RID: 523 RVA: 0x0000C729 File Offset: 0x0000A929
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

	// Token: 0x0600020C RID: 524 RVA: 0x0000C75D File Offset: 0x0000A95D
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/BlurTiltShift_Hole");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000C780 File Offset: 0x0000A980
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
		this.material.SetFloat("_Value3", this.PositionX);
		this.material.SetFloat("_Value4", this.PositionY);
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

	// Token: 0x0600020E RID: 526 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000C926 File Offset: 0x0000AB26
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000323 RID: 803
	public Shader SCShader;

	// Token: 0x04000324 RID: 804
	private float TimeX = 1f;

	// Token: 0x04000325 RID: 805
	private Vector4 ScreenResolution;

	// Token: 0x04000326 RID: 806
	private Material SCMaterial;

	// Token: 0x04000327 RID: 807
	[Range(0f, 20f)]
	public float Amount = 3f;

	// Token: 0x04000328 RID: 808
	[Range(2f, 16f)]
	public int FastFilter = 8;

	// Token: 0x04000329 RID: 809
	[Range(0f, 1f)]
	public float Smooth = 0.5f;

	// Token: 0x0400032A RID: 810
	[Range(0f, 1f)]
	public float Size = 0.2f;

	// Token: 0x0400032B RID: 811
	[Range(-1f, 1f)]
	public float PositionX = 0.5f;

	// Token: 0x0400032C RID: 812
	[Range(-1f, 1f)]
	public float PositionY = 0.5f;
}
