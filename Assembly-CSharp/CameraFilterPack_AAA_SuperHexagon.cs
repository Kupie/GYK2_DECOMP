using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Super Hexagon")]
public class CameraFilterPack_AAA_SuperHexagon : MonoBehaviour
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000076 RID: 118 RVA: 0x00005140 File Offset: 0x00003340
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

	// Token: 0x06000077 RID: 119 RVA: 0x00005174 File Offset: 0x00003374
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Super_Hexagon");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00005198 File Offset: 0x00003398
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
			this.material.SetFloat("_Value", this.HexaSize);
			this.material.SetFloat("_PositionX", this.center.x);
			this.material.SetFloat("_PositionY", this.center.y);
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetFloat("_BorderSize", this._BorderSize);
			this.material.SetColor("_BorderColor", this._BorderColor);
			this.material.SetColor("_HexaColor", this._HexaColor);
			this.material.SetFloat("_AlphaHexa", this._AlphaHexa);
			this.material.SetFloat("_SpotSize", this._SpotSize);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00005308 File Offset: 0x00003508
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000FA RID: 250
	public Shader SCShader;

	// Token: 0x040000FB RID: 251
	[Range(0f, 1f)]
	public float _AlphaHexa = 1f;

	// Token: 0x040000FC RID: 252
	private float TimeX = 1f;

	// Token: 0x040000FD RID: 253
	private Vector4 ScreenResolution;

	// Token: 0x040000FE RID: 254
	private Material SCMaterial;

	// Token: 0x040000FF RID: 255
	[Range(0.2f, 10f)]
	public float HexaSize = 2.5f;

	// Token: 0x04000100 RID: 256
	public float _BorderSize = 1f;

	// Token: 0x04000101 RID: 257
	public Color _BorderColor = new Color(0.75f, 0.75f, 1f, 1f);

	// Token: 0x04000102 RID: 258
	public Color _HexaColor = new Color(0f, 0.5f, 1f, 1f);

	// Token: 0x04000103 RID: 259
	public float _SpotSize = 2.5f;

	// Token: 0x04000104 RID: 260
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x04000105 RID: 261
	public float Radius = 0.25f;
}
