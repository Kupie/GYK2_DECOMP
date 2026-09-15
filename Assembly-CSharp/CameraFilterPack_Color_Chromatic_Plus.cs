using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Chromatic_Plus")]
public class CameraFilterPack_Color_Chromatic_Plus : MonoBehaviour
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000267 RID: 615 RVA: 0x0000E482 File Offset: 0x0000C682
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

	// Token: 0x06000268 RID: 616 RVA: 0x0000E4B6 File Offset: 0x0000C6B6
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Chromatic_Plus");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Smooth);
			this.material.SetFloat("_Distortion", this.Offset);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000E5BA File Offset: 0x0000C7BA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003B5 RID: 949
	public Shader SCShader;

	// Token: 0x040003B6 RID: 950
	private float TimeX = 1f;

	// Token: 0x040003B7 RID: 951
	private Vector4 ScreenResolution;

	// Token: 0x040003B8 RID: 952
	private Material SCMaterial;

	// Token: 0x040003B9 RID: 953
	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	// Token: 0x040003BA RID: 954
	[Range(0.01f, 0.4f)]
	public float Smooth = 0.26f;

	// Token: 0x040003BB RID: 955
	[Range(-0.02f, 0.02f)]
	public float Offset = 0.005f;
}
