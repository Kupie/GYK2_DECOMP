using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Tilt_Shift")]
public class CameraFilterPack_Blur_Tilt_Shift : MonoBehaviour
{
	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000205 RID: 517 RVA: 0x0000C4AC File Offset: 0x0000A6AC
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

	// Token: 0x06000206 RID: 518 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/BlurTiltShift");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000C504 File Offset: 0x0000A704
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
		this.material.SetVector("_ScreenResolution", new Vector2((float)(Screen.width / fastFilter), (float)(Screen.height / fastFilter)));
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

	// Token: 0x06000208 RID: 520 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000C6BE File Offset: 0x0000A8BE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400031A RID: 794
	public Shader SCShader;

	// Token: 0x0400031B RID: 795
	private float TimeX = 1f;

	// Token: 0x0400031C RID: 796
	private Vector4 ScreenResolution;

	// Token: 0x0400031D RID: 797
	private Material SCMaterial;

	// Token: 0x0400031E RID: 798
	[Range(0f, 20f)]
	public float Amount = 3f;

	// Token: 0x0400031F RID: 799
	[Range(2f, 16f)]
	public int FastFilter = 8;

	// Token: 0x04000320 RID: 800
	[Range(0f, 1f)]
	public float Smooth = 0.5f;

	// Token: 0x04000321 RID: 801
	[Range(0f, 1f)]
	public float Size = 0.5f;

	// Token: 0x04000322 RID: 802
	[Range(-1f, 1f)]
	public float Position = 0.5f;
}
