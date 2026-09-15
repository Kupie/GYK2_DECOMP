using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Horror")]
public class CameraFilterPack_TV_Horror : MonoBehaviour
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001CF33 File Offset: 0x0001B133
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

	// Token: 0x060005F2 RID: 1522 RVA: 0x0001CF67 File Offset: 0x0001B167
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_HorrorFX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/TV_Horror");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0001CFA0 File Offset: 0x0001B1A0
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
			this.material.SetFloat("_Value", this.Fade);
			this.material.SetFloat("_Value2", this.Fade_Additive);
			this.material.SetFloat("_Value3", this.Fade_Distortion);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001D0AE File Offset: 0x0001B2AE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007BA RID: 1978
	public Shader SCShader;

	// Token: 0x040007BB RID: 1979
	private float TimeX = 1f;

	// Token: 0x040007BC RID: 1980
	private Vector4 ScreenResolution;

	// Token: 0x040007BD RID: 1981
	private Material SCMaterial;

	// Token: 0x040007BE RID: 1982
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040007BF RID: 1983
	[Range(0f, 1f)]
	private float Fade_Additive;

	// Token: 0x040007C0 RID: 1984
	[Range(0f, 1f)]
	private float Fade_Distortion;

	// Token: 0x040007C1 RID: 1985
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x040007C2 RID: 1986
	private Texture2D Texture2;
}
