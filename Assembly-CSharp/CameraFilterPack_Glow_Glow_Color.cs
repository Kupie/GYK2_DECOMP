using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glow/Glow_Color")]
public class CameraFilterPack_Glow_Glow_Color : MonoBehaviour
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00016E7D File Offset: 0x0001507D
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

	// Token: 0x060004A3 RID: 1187 RVA: 0x00016EB1 File Offset: 0x000150B1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Glow_Glow_Color");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00016ED4 File Offset: 0x000150D4
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
		this.material.SetColor("_GlowColor", this.GlowColor);
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

	// Token: 0x060004A5 RID: 1189 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x000170B2 File Offset: 0x000152B2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000620 RID: 1568
	public Shader SCShader;

	// Token: 0x04000621 RID: 1569
	private float TimeX = 1f;

	// Token: 0x04000622 RID: 1570
	private Vector4 ScreenResolution;

	// Token: 0x04000623 RID: 1571
	private Material SCMaterial;

	// Token: 0x04000624 RID: 1572
	[Range(0f, 20f)]
	public float Amount = 4f;

	// Token: 0x04000625 RID: 1573
	[Range(2f, 16f)]
	public int FastFilter = 4;

	// Token: 0x04000626 RID: 1574
	[Range(0f, 1f)]
	public float Threshold = 0.5f;

	// Token: 0x04000627 RID: 1575
	[Range(0f, 3f)]
	public float Intensity = 2.25f;

	// Token: 0x04000628 RID: 1576
	[Range(-1f, 1f)]
	public float Precision = 0.56f;

	// Token: 0x04000629 RID: 1577
	public Color GlowColor = new Color(0f, 0.7f, 1f, 1f);
}
