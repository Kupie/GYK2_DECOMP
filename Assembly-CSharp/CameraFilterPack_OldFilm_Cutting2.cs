using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Cutting 2")]
public class CameraFilterPack_OldFilm_Cutting2 : MonoBehaviour
{
	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001AC4D File Offset: 0x00018E4D
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

	// Token: 0x0600056D RID: 1389 RVA: 0x0001AC81 File Offset: 0x00018E81
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_OldFilm2") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/OldFilm_Cutting2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001ACB8 File Offset: 0x00018EB8
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
			this.material.SetFloat("_Value", 2f - this.Luminosity);
			this.material.SetFloat("_Value2", 1f - this.Vignette);
			this.material.SetFloat("_Value3", this.Negative);
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001ADA5 File Offset: 0x00018FA5
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000721 RID: 1825
	public Shader SCShader;

	// Token: 0x04000722 RID: 1826
	private float TimeX = 1f;

	// Token: 0x04000723 RID: 1827
	[Range(0f, 10f)]
	public float Speed = 5f;

	// Token: 0x04000724 RID: 1828
	[Range(0f, 2f)]
	public float Luminosity = 1f;

	// Token: 0x04000725 RID: 1829
	[Range(0f, 1f)]
	public float Vignette = 1f;

	// Token: 0x04000726 RID: 1830
	[Range(0f, 1f)]
	public float Negative;

	// Token: 0x04000727 RID: 1831
	private Material SCMaterial;

	// Token: 0x04000728 RID: 1832
	private Texture2D Texture2;
}
