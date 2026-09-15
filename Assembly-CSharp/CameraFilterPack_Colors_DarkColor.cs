using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/DarkColor")]
public class CameraFilterPack_Colors_DarkColor : MonoBehaviour
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000237 RID: 567 RVA: 0x0000D91B File Offset: 0x0000BB1B
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

	// Token: 0x06000238 RID: 568 RVA: 0x0000D94F File Offset: 0x0000BB4F
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_DarkColor");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000D970 File Offset: 0x0000BB70
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
			this.material.SetFloat("_Value", this.Alpha);
			this.material.SetFloat("_Value2", this.Colors);
			this.material.SetFloat("_Value3", this.Green_Mod);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000DA68 File Offset: 0x0000BC68
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400037F RID: 895
	public Shader SCShader;

	// Token: 0x04000380 RID: 896
	private float TimeX = 1f;

	// Token: 0x04000381 RID: 897
	private Vector4 ScreenResolution;

	// Token: 0x04000382 RID: 898
	private Material SCMaterial;

	// Token: 0x04000383 RID: 899
	[Range(-5f, 5f)]
	public float Alpha = 1f;

	// Token: 0x04000384 RID: 900
	[Range(0f, 16f)]
	private float Colors = 11f;

	// Token: 0x04000385 RID: 901
	[Range(-1f, 1f)]
	private float Green_Mod = 1f;

	// Token: 0x04000386 RID: 902
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
