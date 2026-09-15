using System;
using UnityEngine;

// Token: 0x02000082 RID: 130
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/EnhancedComics")]
public class CameraFilterPack_Drawing_EnhancedComics : MonoBehaviour
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000333 RID: 819 RVA: 0x0001149E File Offset: 0x0000F69E
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

	// Token: 0x06000334 RID: 820 RVA: 0x000114D2 File Offset: 0x0000F6D2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_EnhancedComics");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000114F4 File Offset: 0x0000F6F4
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
			this.material.SetFloat("_DotSize", this.DotSize);
			this.material.SetFloat("_ColorR", this._ColorR);
			this.material.SetFloat("_ColorG", this._ColorG);
			this.material.SetFloat("_ColorB", this._ColorB);
			this.material.SetFloat("_Blood", this._Blood);
			this.material.SetColor("_ColorRGB", this.ColorRGB);
			this.material.SetFloat("_SmoothStart", this._SmoothStart);
			this.material.SetFloat("_SmoothEnd", this._SmoothEnd);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00011617 File Offset: 0x0000F817
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400048C RID: 1164
	public Shader SCShader;

	// Token: 0x0400048D RID: 1165
	private float TimeX = 1f;

	// Token: 0x0400048E RID: 1166
	private Material SCMaterial;

	// Token: 0x0400048F RID: 1167
	[Range(0f, 1f)]
	public float DotSize = 0.15f;

	// Token: 0x04000490 RID: 1168
	[Range(0f, 1f)]
	public float _ColorR = 0.9f;

	// Token: 0x04000491 RID: 1169
	[Range(0f, 1f)]
	public float _ColorG = 0.4f;

	// Token: 0x04000492 RID: 1170
	[Range(0f, 1f)]
	public float _ColorB = 0.4f;

	// Token: 0x04000493 RID: 1171
	[Range(0f, 1f)]
	public float _Blood = 0.5f;

	// Token: 0x04000494 RID: 1172
	[Range(0f, 1f)]
	public float _SmoothStart = 0.02f;

	// Token: 0x04000495 RID: 1173
	[Range(0f, 1f)]
	public float _SmoothEnd = 0.1f;

	// Token: 0x04000496 RID: 1174
	public Color ColorRGB = new Color(1f, 0f, 0f);
}
