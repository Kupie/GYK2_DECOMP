using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Color_YUV")]
public class CameraFilterPack_Color_YUV : MonoBehaviour
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000297 RID: 663 RVA: 0x0000EEF3 File Offset: 0x0000D0F3
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

	// Token: 0x06000298 RID: 664 RVA: 0x0000EF27 File Offset: 0x0000D127
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_YUV");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000EF48 File Offset: 0x0000D148
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
			this.material.SetFloat("_Y", this._Y);
			this.material.SetFloat("_U", this._U);
			this.material.SetFloat("_V", this._V);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000F02A File Offset: 0x0000D22A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003DF RID: 991
	public Shader SCShader;

	// Token: 0x040003E0 RID: 992
	private float TimeX = 1f;

	// Token: 0x040003E1 RID: 993
	private Vector4 ScreenResolution;

	// Token: 0x040003E2 RID: 994
	private Material SCMaterial;

	// Token: 0x040003E3 RID: 995
	[Range(-1f, 1f)]
	public float _Y;

	// Token: 0x040003E4 RID: 996
	[Range(-1f, 1f)]
	public float _U;

	// Token: 0x040003E5 RID: 997
	[Range(-1f, 1f)]
	public float _V;
}
