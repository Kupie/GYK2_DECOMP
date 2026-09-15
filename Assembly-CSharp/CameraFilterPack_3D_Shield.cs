using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Shield")]
public class CameraFilterPack_3D_Shield : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00003F0C File Offset: 0x0000210C
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

	// Token: 0x0600004D RID: 77 RVA: 0x00003F40 File Offset: 0x00002140
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Shield");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003F64 File Offset: 0x00002164
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
			this.material.SetFloat("_LightIntensity", this.LightIntensity * 64f);
			this.material.SetFloat("_FadeShield", this._FadeShield);
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.Speed_X);
			this.material.SetFloat("_Value3", this.Speed_Y);
			this.material.SetFloat("_Value4", this.Intensity);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			this.material.SetFloat("_FarCamera", 1000f / farClipPlane);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004168 File Offset: 0x00002368
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400009A RID: 154
	public Shader SCShader;

	// Token: 0x0400009B RID: 155
	private float TimeX = 1f;

	// Token: 0x0400009C RID: 156
	private Vector4 ScreenResolution;

	// Token: 0x0400009D RID: 157
	private Material SCMaterial;

	// Token: 0x0400009E RID: 158
	[Range(0f, 100f)]
	public float _FixDistance = 1.5f;

	// Token: 0x0400009F RID: 159
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.4f;

	// Token: 0x040000A0 RID: 160
	[Range(0f, 0.5f)]
	public float _Size = 0.5f;

	// Token: 0x040000A1 RID: 161
	[Range(0f, 1f)]
	public float _FadeShield = 0.75f;

	// Token: 0x040000A2 RID: 162
	[Range(-0.2f, 0.2f)]
	public float LightIntensity = 0.025f;

	// Token: 0x040000A3 RID: 163
	public bool AutoAnimatedNear;

	// Token: 0x040000A4 RID: 164
	[Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	// Token: 0x040000A5 RID: 165
	[Range(0f, 10f)]
	public float Speed = 0.2f;

	// Token: 0x040000A6 RID: 166
	[Range(0f, 10f)]
	public float Speed_X = 0.2f;

	// Token: 0x040000A7 RID: 167
	[Range(0f, 1f)]
	public float Speed_Y = 0.3f;

	// Token: 0x040000A8 RID: 168
	[Range(0f, 10f)]
	public float Intensity = 2.4f;

	// Token: 0x040000A9 RID: 169
	public static Color ChangeColorRGB;
}
