using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Planet Mars")]
public class CameraFilterPack_TV_PlanetMars : MonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001D8C3 File Offset: 0x0001BAC3
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

	// Token: 0x0600061C RID: 1564 RVA: 0x0001D8F7 File Offset: 0x0001BAF7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_PlanetMars");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0001D918 File Offset: 0x0001BB18
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0001D9CE File Offset: 0x0001BBCE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007E4 RID: 2020
	public Shader SCShader;

	// Token: 0x040007E5 RID: 2021
	private Vector4 ScreenResolution;

	// Token: 0x040007E6 RID: 2022
	private float TimeX = 1f;

	// Token: 0x040007E7 RID: 2023
	[Range(-10f, 10f)]
	public float Distortion = 1f;

	// Token: 0x040007E8 RID: 2024
	private Material SCMaterial;
}
