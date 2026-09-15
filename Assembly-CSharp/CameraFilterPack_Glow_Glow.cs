using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glow/Glow")]
public class CameraFilterPack_Glow_Glow : MonoBehaviour
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x00016BF1 File Offset: 0x00014DF1
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

	// Token: 0x0600049D RID: 1181 RVA: 0x00016C25 File Offset: 0x00014E25
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Glow_Glow");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00016C48 File Offset: 0x00014E48
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
		this.material.SetFloat("_Value1", this.Threshold);
		this.material.SetFloat("_Value2", this.Intensity);
		this.material.SetFloat("_Value3", this.Precision);
		this.material.SetVector("_ScreenResolution", new Vector2((float)(Screen.width / fastFilter), (float)(Screen.height / fastFilter)));
		int num = sourceTexture.width / fastFilter;
		int num2 = sourceTexture.height / fastFilter;
		if (this.FastFilter > 1)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(sourceTexture, temporary, this.material, 3);
			Graphics.Blit(temporary, temporary2, this.material, 2);
			Graphics.Blit(temporary2, temporary, this.material, 0);
			this.material.SetFloat("_Amount", this.Amount * 2f);
			Graphics.Blit(temporary, temporary2, this.material, 2);
			Graphics.Blit(temporary2, temporary, this.material, 0);
			this.material.SetTexture("_MainTex2", temporary);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			Graphics.Blit(sourceTexture, destTexture, this.material, 1);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture, this.material, 0);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00016E10 File Offset: 0x00015010
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000617 RID: 1559
	public Shader SCShader;

	// Token: 0x04000618 RID: 1560
	private float TimeX = 1f;

	// Token: 0x04000619 RID: 1561
	private Vector4 ScreenResolution;

	// Token: 0x0400061A RID: 1562
	private Material SCMaterial;

	// Token: 0x0400061B RID: 1563
	[Range(0f, 20f)]
	public float Amount = 4f;

	// Token: 0x0400061C RID: 1564
	[Range(2f, 16f)]
	public int FastFilter = 4;

	// Token: 0x0400061D RID: 1565
	[Range(0f, 1f)]
	public float Threshold = 0.5f;

	// Token: 0x0400061E RID: 1566
	[Range(0f, 1f)]
	public float Intensity = 0.75f;

	// Token: 0x0400061F RID: 1567
	[Range(-1f, 1f)]
	public float Precision = 0.56f;
}
