using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Fog")]
public class CameraFilterPack_Atmosphere_Fog : MonoBehaviour
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000094 RID: 148 RVA: 0x000059CB File Offset: 0x00003BCB
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

	// Token: 0x06000095 RID: 149 RVA: 0x000059FF File Offset: 0x00003BFF
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Fog");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00005A38 File Offset: 0x00003C38
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
			this.material.SetFloat("_DepthLevel", this.Fade);
			this.material.SetFloat("_Near", this._Near);
			this.material.SetFloat("_Far", this._Far);
			this.material.SetColor("_ColorRGB", this.FogColor);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00005B52 File Offset: 0x00003D52
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000122 RID: 290
	public Shader SCShader;

	// Token: 0x04000123 RID: 291
	private float TimeX = 1f;

	// Token: 0x04000124 RID: 292
	private Vector4 ScreenResolution;

	// Token: 0x04000125 RID: 293
	private Material SCMaterial;

	// Token: 0x04000126 RID: 294
	[Range(0f, 1f)]
	public float _Near;

	// Token: 0x04000127 RID: 295
	[Range(0f, 1f)]
	public float _Far = 0.05f;

	// Token: 0x04000128 RID: 296
	public Color FogColor = new Color(0.4f, 0.4f, 0.4f, 1f);

	// Token: 0x04000129 RID: 297
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400012A RID: 298
	public static Color ChangeColorRGB;

	// Token: 0x0400012B RID: 299
	private Texture2D Texture2;
}
