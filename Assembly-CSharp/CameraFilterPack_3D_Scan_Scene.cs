using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Scan_Scene")]
public class CameraFilterPack_3D_Scan_Scene : MonoBehaviour
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000046 RID: 70 RVA: 0x00003C83 File Offset: 0x00001E83
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

	// Token: 0x06000047 RID: 71 RVA: 0x00003CB7 File Offset: 0x00001EB7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Scan_Scene");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003CD8 File Offset: 0x00001ED8
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
			if (this.AutoAnimatedNear)
			{
				this._Distance += Time.deltaTime * this.AutoAnimatedNearSpeed;
				if (this._Distance > 1f)
				{
					this._Distance = 0f;
				}
				if (this._Distance < 0f)
				{
					this._Distance = 1f;
				}
				this.material.SetFloat("_Near", this._Distance);
			}
			else
			{
				this.material.SetFloat("_Near", this._Distance);
			}
			this.material.SetFloat("_Far", this._Size);
			this.material.SetColor("_ColorRGB", this.ScanColor);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003E7E File Offset: 0x0000207E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400008D RID: 141
	public Shader SCShader;

	// Token: 0x0400008E RID: 142
	private float TimeX = 1f;

	// Token: 0x0400008F RID: 143
	private Vector4 ScreenResolution;

	// Token: 0x04000090 RID: 144
	private Material SCMaterial;

	// Token: 0x04000091 RID: 145
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x04000092 RID: 146
	[Range(0f, 0.99f)]
	public float _Distance = 1f;

	// Token: 0x04000093 RID: 147
	[Range(0f, 0.1f)]
	public float _Size = 0.01f;

	// Token: 0x04000094 RID: 148
	public bool AutoAnimatedNear;

	// Token: 0x04000095 RID: 149
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 1f;

	// Token: 0x04000096 RID: 150
	public Color ScanColor = new Color(2f, 0f, 0f, 1f);

	// Token: 0x04000097 RID: 151
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000098 RID: 152
	public static Color ChangeColorRGB;

	// Token: 0x04000099 RID: 153
	private Texture2D Texture2;
}
