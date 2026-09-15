using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Matrix")]
public class CameraFilterPack_3D_Matrix : MonoBehaviour
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000034 RID: 52 RVA: 0x000034C9 File Offset: 0x000016C9
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

	// Token: 0x06000035 RID: 53 RVA: 0x000034FD File Offset: 0x000016FD
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_3D_Matrix1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Matrix");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003534 File Offset: 0x00001734
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

	// Token: 0x06000037 RID: 55 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000036A3 File Offset: 0x000018A3
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000067 RID: 103
	public Shader SCShader;

	// Token: 0x04000068 RID: 104
	private float TimeX = 1f;

	// Token: 0x04000069 RID: 105
	private Vector4 ScreenResolution;

	// Token: 0x0400006A RID: 106
	private Material SCMaterial;

	// Token: 0x0400006B RID: 107
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x0400006C RID: 108
	[Range(-5f, 5f)]
	public float LightIntensity = 1f;

	// Token: 0x0400006D RID: 109
	[Range(0f, 6f)]
	public float MatrixSize = 1f;

	// Token: 0x0400006E RID: 110
	[Range(-4f, 4f)]
	public float MatrixSpeed = 1f;

	// Token: 0x0400006F RID: 111
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000070 RID: 112
	public Color _MatrixColor = new Color(0f, 1f, 0f, 1f);

	// Token: 0x04000071 RID: 113
	public static Color ChangeColorRGB;

	// Token: 0x04000072 RID: 114
	private Texture2D Texture2;
}
