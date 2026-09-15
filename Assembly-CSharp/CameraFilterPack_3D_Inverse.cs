using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Inverse")]
public class CameraFilterPack_3D_Inverse : MonoBehaviour
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00003273 File Offset: 0x00001473
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

	// Token: 0x0600002F RID: 47 RVA: 0x000032A7 File Offset: 0x000014A7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Inverse");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000032C8 File Offset: 0x000014C8
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
			this.material.SetFloat("_LightIntensity", this.LightIntensity);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003458 File Offset: 0x00001658
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400005C RID: 92
	public Shader SCShader;

	// Token: 0x0400005D RID: 93
	private float TimeX = 1f;

	// Token: 0x0400005E RID: 94
	private Vector4 ScreenResolution;

	// Token: 0x0400005F RID: 95
	private Material SCMaterial;

	// Token: 0x04000060 RID: 96
	[Range(0f, 100f)]
	public float _FixDistance = 1.5f;

	// Token: 0x04000061 RID: 97
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.4f;

	// Token: 0x04000062 RID: 98
	[Range(0f, 0.5f)]
	public float _Size = 0.5f;

	// Token: 0x04000063 RID: 99
	[Range(0f, 1f)]
	public float LightIntensity = 1f;

	// Token: 0x04000064 RID: 100
	public bool AutoAnimatedNear;

	// Token: 0x04000065 RID: 101
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x04000066 RID: 102
	public static Color ChangeColorRGB;
}
