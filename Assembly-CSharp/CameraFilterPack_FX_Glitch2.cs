using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Glitch2")]
public class CameraFilterPack_FX_Glitch2 : MonoBehaviour
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x000156CA File Offset: 0x000138CA
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

	// Token: 0x0600043D RID: 1085 RVA: 0x000156FE File Offset: 0x000138FE
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Glitch2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00015720 File Offset: 0x00013920
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
			this.material.SetFloat("_Glitch", this.Glitch);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x000157D6 File Offset: 0x000139D6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005BE RID: 1470
	public Shader SCShader;

	// Token: 0x040005BF RID: 1471
	private float TimeX = 1f;

	// Token: 0x040005C0 RID: 1472
	private Vector4 ScreenResolution;

	// Token: 0x040005C1 RID: 1473
	private Material SCMaterial;

	// Token: 0x040005C2 RID: 1474
	[Range(0f, 1f)]
	public float Glitch = 1f;
}
