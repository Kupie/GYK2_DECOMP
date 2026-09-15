using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/BlackHole")]
public class CameraFilterPack_3D_BlackHole : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000010 RID: 16 RVA: 0x0000261D File Offset: 0x0000081D
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

	// Token: 0x06000011 RID: 17 RVA: 0x00002651 File Offset: 0x00000851
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_BlackHole");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002674 File Offset: 0x00000874
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
			this.material.SetFloat("_DistortionLevel", this.DistortionLevel);
			this.material.SetFloat("_DistortionSize", this.DistortionSize);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000281A File Offset: 0x00000A1A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400001D RID: 29
	public Shader SCShader;

	// Token: 0x0400001E RID: 30
	private float TimeX = 1f;

	// Token: 0x0400001F RID: 31
	private Vector4 ScreenResolution;

	// Token: 0x04000020 RID: 32
	private Material SCMaterial;

	// Token: 0x04000021 RID: 33
	[Range(0f, 100f)]
	public float _FixDistance = 5f;

	// Token: 0x04000022 RID: 34
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.05f;

	// Token: 0x04000023 RID: 35
	[Range(0f, 1f)]
	public float _Size = 0.25f;

	// Token: 0x04000024 RID: 36
	[Range(-2f, 2f)]
	public float DistortionLevel = 1.2f;

	// Token: 0x04000025 RID: 37
	[Range(0f, 1f)]
	public float DistortionSize;

	// Token: 0x04000026 RID: 38
	public bool AutoAnimatedNear;

	// Token: 0x04000027 RID: 39
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x04000028 RID: 40
	public static Color ChangeColorRGB;
}
