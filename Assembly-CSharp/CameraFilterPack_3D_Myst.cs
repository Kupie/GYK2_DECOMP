using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Myst")]
public class CameraFilterPack_3D_Myst : MonoBehaviour
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000040 RID: 64 RVA: 0x000039AC File Offset: 0x00001BAC
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

	// Token: 0x06000041 RID: 65 RVA: 0x000039E0 File Offset: 0x00001BE0
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_3D_Myst1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Myst");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003A18 File Offset: 0x00001C18
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
			if (this.AutoAnimatedNear)
			{
				this._Distance += Time.deltaTime * this.AutoAnimatedNearSpeed;
				if (this._Distance > 1f)
				{
					this._Distance = -1f;
				}
				if (this._Distance < -1f)
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
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("_DistortionLevel", this.DistortionLevel * 28f);
			this.material.SetFloat("_DistortionSize", this.DistortionSize * 16f);
			this.material.SetFloat("_LightIntensity", this.LightIntensity * 64f);
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

	// Token: 0x06000043 RID: 67 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003BFC File Offset: 0x00001DFC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400007F RID: 127
	public Shader SCShader;

	// Token: 0x04000080 RID: 128
	private float TimeX = 1f;

	// Token: 0x04000081 RID: 129
	private Vector4 ScreenResolution;

	// Token: 0x04000082 RID: 130
	private Material SCMaterial;

	// Token: 0x04000083 RID: 131
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x04000084 RID: 132
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.5f;

	// Token: 0x04000085 RID: 133
	[Range(0f, 0.5f)]
	public float _Size = 0.1f;

	// Token: 0x04000086 RID: 134
	[Range(0f, 10f)]
	public float DistortionLevel = 1.2f;

	// Token: 0x04000087 RID: 135
	[Range(0.1f, 10f)]
	public float DistortionSize = 1.4f;

	// Token: 0x04000088 RID: 136
	[Range(-2f, 4f)]
	public float LightIntensity = 0.08f;

	// Token: 0x04000089 RID: 137
	public bool AutoAnimatedNear;

	// Token: 0x0400008A RID: 138
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x0400008B RID: 139
	private Texture2D Texture2;

	// Token: 0x0400008C RID: 140
	public static Color ChangeColorRGB;
}
