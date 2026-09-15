using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Dithering2x2")]
public class CameraFilterPack_Blur_Dithering2x2 : MonoBehaviour
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000B5B3 File Offset: 0x000097B3
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

	// Token: 0x060001CA RID: 458 RVA: 0x0000B5E7 File Offset: 0x000097E7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Dithering2x2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000B608 File Offset: 0x00009808
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
			this.material.SetFloat("_Level", (float)this.Level);
			this.material.SetVector("_Distance", this.Distance);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000B6DA File Offset: 0x000098DA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002D8 RID: 728
	public Shader SCShader;

	// Token: 0x040002D9 RID: 729
	private float TimeX = 1f;

	// Token: 0x040002DA RID: 730
	private Vector4 ScreenResolution;

	// Token: 0x040002DB RID: 731
	private Material SCMaterial;

	// Token: 0x040002DC RID: 732
	[Range(2f, 16f)]
	public int Level = 4;

	// Token: 0x040002DD RID: 733
	public Vector2 Distance = new Vector2(30f, 0f);
}
