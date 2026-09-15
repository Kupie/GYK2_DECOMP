using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Snow")]
public class CameraFilterPack_3D_Snow : MonoBehaviour
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000052 RID: 82 RVA: 0x00004210 File Offset: 0x00002410
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

	// Token: 0x06000053 RID: 83 RVA: 0x00004244 File Offset: 0x00002444
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Blizzard1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/3D_Snow");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0000427C File Offset: 0x0000247C
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
			this.material.SetFloat("_Value", this.Fade);
			this.material.SetFloat("_Value2", this.Intensity);
			this.material.SetFloat("_Value4", this.Speed * 6f);
			this.material.SetFloat("_Value5", this.Size);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("Drop_Near", this.Snow_Near);
			this.material.SetFloat("Drop_Far", this.Snow_Far);
			this.material.SetFloat("Drop_With_Obj", this.SnowWithoutObject);
			this.material.SetFloat("Myst", this.Myst);
			this.material.SetColor("Myst_Color", this.Myst_Color);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004420 File Offset: 0x00002620
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000AA RID: 170
	public Shader SCShader;

	// Token: 0x040000AB RID: 171
	private float TimeX = 1f;

	// Token: 0x040000AC RID: 172
	private Vector4 ScreenResolution;

	// Token: 0x040000AD RID: 173
	private Material SCMaterial;

	// Token: 0x040000AE RID: 174
	[Range(0f, 100f)]
	public float _FixDistance = 5f;

	// Token: 0x040000AF RID: 175
	[Range(-0.5f, 0.99f)]
	public float Snow_Near = 0.08f;

	// Token: 0x040000B0 RID: 176
	[Range(0f, 1f)]
	public float Snow_Far = 0.55f;

	// Token: 0x040000B1 RID: 177
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040000B2 RID: 178
	[Range(0f, 2f)]
	public float Intensity = 1f;

	// Token: 0x040000B3 RID: 179
	[Range(0.4f, 2f)]
	public float Size = 1f;

	// Token: 0x040000B4 RID: 180
	[Range(0f, 0.5f)]
	public float Speed = 0.275f;

	// Token: 0x040000B5 RID: 181
	[Range(0f, 1f)]
	public float SnowWithoutObject = 1f;

	// Token: 0x040000B6 RID: 182
	[Range(0f, 1f)]
	public float Myst;

	// Token: 0x040000B7 RID: 183
	public Color Myst_Color = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x040000B8 RID: 184
	private Texture2D Texture2;
}
