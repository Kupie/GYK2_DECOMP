using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/WideScreenVertical")]
public class CameraFilterPack_TV_WideScreenVertical : MonoBehaviour
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001EBFD File Offset: 0x0001CDFD
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

	// Token: 0x06000670 RID: 1648 RVA: 0x0001EC31 File Offset: 0x0001CE31
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_WideScreenVertical");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0001EC54 File Offset: 0x0001CE54
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
			this.material.SetFloat("_Value3", this.StretchX);
			this.material.SetFloat("_Value4", this.StretchY);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0001ED4C File Offset: 0x0001CF4C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000839 RID: 2105
	public Shader SCShader;

	// Token: 0x0400083A RID: 2106
	private float TimeX = 1f;

	// Token: 0x0400083B RID: 2107
	private Vector4 ScreenResolution;

	// Token: 0x0400083C RID: 2108
	private Material SCMaterial;

	// Token: 0x0400083D RID: 2109
	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	// Token: 0x0400083E RID: 2110
	[Range(0.001f, 0.4f)]
	public float Smooth = 0.01f;

	// Token: 0x0400083F RID: 2111
	[Range(0f, 10f)]
	private float StretchX = 1f;

	// Token: 0x04000840 RID: 2112
	[Range(0f, 10f)]
	private float StretchY = 1f;
}
