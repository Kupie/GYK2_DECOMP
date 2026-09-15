using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Binary")]
public class CameraFilterPack_3D_Binary : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000023AA File Offset: 0x000005AA
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

	// Token: 0x0600000B RID: 11 RVA: 0x000023DE File Offset: 0x000005DE
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_3D_Binary1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Binary");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002414 File Offset: 0x00000614
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
			this.material.SetFloat("_FadeFromBinary", this.FadeFromBinary);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("_MatrixSize", this.MatrixSize);
			this.material.SetColor("_MatrixColor", this._MatrixColor);
			this.material.SetFloat("_MatrixSpeed", this.MatrixSpeed * 2f);
			this.material.SetFloat("_LightIntensity", this.LightIntensity);
			this.material.SetTexture("_MainTex2", this.Texture2);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002599 File Offset: 0x00000799
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000010 RID: 16
	public Shader SCShader;

	// Token: 0x04000011 RID: 17
	private float TimeX = 1f;

	// Token: 0x04000012 RID: 18
	private Vector4 ScreenResolution;

	// Token: 0x04000013 RID: 19
	private Material SCMaterial;

	// Token: 0x04000014 RID: 20
	[Range(0f, 100f)]
	public float _FixDistance = 2f;

	// Token: 0x04000015 RID: 21
	[Range(-5f, 5f)]
	public float LightIntensity;

	// Token: 0x04000016 RID: 22
	[Range(0f, 8f)]
	public float MatrixSize = 2f;

	// Token: 0x04000017 RID: 23
	[Range(-4f, 4f)]
	public float MatrixSpeed = 1f;

	// Token: 0x04000018 RID: 24
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000019 RID: 25
	[Range(0f, 1f)]
	public float FadeFromBinary;

	// Token: 0x0400001A RID: 26
	public Color _MatrixColor = new Color(1f, 0.3f, 0.3f, 1f);

	// Token: 0x0400001B RID: 27
	public static Color ChangeColorRGB;

	// Token: 0x0400001C RID: 28
	private Texture2D Texture2;
}
