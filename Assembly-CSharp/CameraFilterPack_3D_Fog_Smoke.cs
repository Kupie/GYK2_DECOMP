using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Fog_Smoke")]
public class CameraFilterPack_3D_Fog_Smoke : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000022 RID: 34 RVA: 0x00002D9F File Offset: 0x00000F9F
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

	// Token: 0x06000023 RID: 35 RVA: 0x00002DD3 File Offset: 0x00000FD3
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_3D_Matrix1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Fog_Smoke");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002E0C File Offset: 0x0000100C
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

	// Token: 0x06000025 RID: 37 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002F7B File Offset: 0x0000117B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000042 RID: 66
	public Shader SCShader;

	// Token: 0x04000043 RID: 67
	private float TimeX = 1f;

	// Token: 0x04000044 RID: 68
	private Vector4 ScreenResolution;

	// Token: 0x04000045 RID: 69
	private Material SCMaterial;

	// Token: 0x04000046 RID: 70
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x04000047 RID: 71
	[Range(-5f, 5f)]
	public float LightIntensity = 1f;

	// Token: 0x04000048 RID: 72
	[Range(0f, 2f)]
	public float MatrixSize = 1f;

	// Token: 0x04000049 RID: 73
	[Range(-4f, 4f)]
	public float MatrixSpeed = 1f;

	// Token: 0x0400004A RID: 74
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400004B RID: 75
	public Color _MatrixColor = new Color(0f, 1f, 0f, 1f);

	// Token: 0x0400004C RID: 76
	public static Color ChangeColorRGB;

	// Token: 0x0400004D RID: 77
	private Texture2D Texture2;
}
