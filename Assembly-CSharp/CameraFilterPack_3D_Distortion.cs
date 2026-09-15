using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Distortion")]
public class CameraFilterPack_3D_Distortion : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600001C RID: 28 RVA: 0x00002AF4 File Offset: 0x00000CF4
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

	// Token: 0x0600001D RID: 29 RVA: 0x00002B28 File Offset: 0x00000D28
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Distortion");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002B4C File Offset: 0x00000D4C
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
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002D1A File Offset: 0x00000F1A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000035 RID: 53
	public Shader SCShader;

	// Token: 0x04000036 RID: 54
	private float TimeX = 1f;

	// Token: 0x04000037 RID: 55
	private Vector4 ScreenResolution;

	// Token: 0x04000038 RID: 56
	private Material SCMaterial;

	// Token: 0x04000039 RID: 57
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x0400003A RID: 58
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.5f;

	// Token: 0x0400003B RID: 59
	[Range(0f, 0.5f)]
	public float _Size = 0.1f;

	// Token: 0x0400003C RID: 60
	[Range(0f, 10f)]
	public float DistortionLevel = 1.2f;

	// Token: 0x0400003D RID: 61
	[Range(0.1f, 10f)]
	public float DistortionSize = 1.4f;

	// Token: 0x0400003E RID: 62
	[Range(-2f, 4f)]
	public float LightIntensity = 0.08f;

	// Token: 0x0400003F RID: 63
	public bool AutoAnimatedNear;

	// Token: 0x04000040 RID: 64
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x04000041 RID: 65
	public static Color ChangeColorRGB;
}
