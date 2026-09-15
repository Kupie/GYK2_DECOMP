using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Computer")]
public class CameraFilterPack_3D_Computer : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002889 File Offset: 0x00000A89
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

	// Token: 0x06000017 RID: 23 RVA: 0x000028BD File Offset: 0x00000ABD
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_3D_Computer1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Computer");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000028F4 File Offset: 0x00000AF4
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

	// Token: 0x06000019 RID: 25 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002A63 File Offset: 0x00000C63
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000029 RID: 41
	public Shader SCShader;

	// Token: 0x0400002A RID: 42
	private float TimeX = 1f;

	// Token: 0x0400002B RID: 43
	private Vector4 ScreenResolution;

	// Token: 0x0400002C RID: 44
	private Material SCMaterial;

	// Token: 0x0400002D RID: 45
	[Range(0f, 100f)]
	public float _FixDistance = 2f;

	// Token: 0x0400002E RID: 46
	[Range(-5f, 5f)]
	public float LightIntensity = 1f;

	// Token: 0x0400002F RID: 47
	[Range(0f, 8f)]
	public float MatrixSize = 2f;

	// Token: 0x04000030 RID: 48
	[Range(-4f, 4f)]
	public float MatrixSpeed = 0.1f;

	// Token: 0x04000031 RID: 49
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000032 RID: 50
	public Color _MatrixColor = new Color(0f, 0.5f, 1f, 1f);

	// Token: 0x04000033 RID: 51
	public static Color ChangeColorRGB;

	// Token: 0x04000034 RID: 52
	private Texture2D Texture2;
}
