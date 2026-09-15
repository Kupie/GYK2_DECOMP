using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Mirror")]
public class CameraFilterPack_3D_Mirror : MonoBehaviour
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00003734 File Offset: 0x00001934
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

	// Token: 0x0600003B RID: 59 RVA: 0x00003768 File Offset: 0x00001968
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Mirror");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000378C File Offset: 0x0000198C
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
			this.material.SetFloat("Fade", this.Fade);
			this.material.SetFloat("Lightning", this.Lightning);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003932 File Offset: 0x00001B32
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000073 RID: 115
	public Shader SCShader;

	// Token: 0x04000074 RID: 116
	private float TimeX = 1f;

	// Token: 0x04000075 RID: 117
	private Vector4 ScreenResolution;

	// Token: 0x04000076 RID: 118
	private Material SCMaterial;

	// Token: 0x04000077 RID: 119
	[Range(0f, 100f)]
	public float _FixDistance = 1.5f;

	// Token: 0x04000078 RID: 120
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.4f;

	// Token: 0x04000079 RID: 121
	[Range(0f, 0.5f)]
	public float _Size = 0.5f;

	// Token: 0x0400007A RID: 122
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400007B RID: 123
	[Range(0f, 2f)]
	public float Lightning = 2f;

	// Token: 0x0400007C RID: 124
	public bool AutoAnimatedNear;

	// Token: 0x0400007D RID: 125
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x0400007E RID: 126
	public static Color ChangeColorRGB;
}
