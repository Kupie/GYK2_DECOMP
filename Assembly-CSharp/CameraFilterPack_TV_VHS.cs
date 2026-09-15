using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/VHS")]
public class CameraFilterPack_TV_VHS : MonoBehaviour
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001DF1A File Offset: 0x0001C11A
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

	// Token: 0x0600063A RID: 1594 RVA: 0x0001DF4E File Offset: 0x0001C14E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_VHS");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0001DF70 File Offset: 0x0001C170
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
			this.material.SetFloat("_Value", this.Cryptage);
			this.material.SetFloat("_Value2", this.Parasite);
			this.material.SetFloat("_Value3", this.Calibrage);
			this.material.SetFloat("_Value4", this.WhiteParasite);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001E068 File Offset: 0x0001C268
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007FE RID: 2046
	public Shader SCShader;

	// Token: 0x040007FF RID: 2047
	private float TimeX = 1f;

	// Token: 0x04000800 RID: 2048
	private Vector4 ScreenResolution;

	// Token: 0x04000801 RID: 2049
	private Material SCMaterial;

	// Token: 0x04000802 RID: 2050
	[Range(1f, 256f)]
	public float Cryptage = 64f;

	// Token: 0x04000803 RID: 2051
	[Range(1f, 100f)]
	public float Parasite = 32f;

	// Token: 0x04000804 RID: 2052
	[Range(0f, 3f)]
	public float Calibrage;

	// Token: 0x04000805 RID: 2053
	[Range(0f, 1f)]
	public float WhiteParasite = 1f;
}
