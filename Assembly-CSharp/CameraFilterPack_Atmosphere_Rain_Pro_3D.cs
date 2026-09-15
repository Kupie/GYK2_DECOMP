using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Rain_Pro_3D")]
public class CameraFilterPack_Atmosphere_Rain_Pro_3D : MonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000A6 RID: 166 RVA: 0x00006066 File Offset: 0x00004266
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

	// Token: 0x060000A7 RID: 167 RVA: 0x0000609A File Offset: 0x0000429A
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Rain_Pro_3D");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000060D0 File Offset: 0x000042D0
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
			if (this.DirectionFollowCameraZ)
			{
				float z = base.GetComponent<Camera>().transform.rotation.z;
				if (z > 0f && z < 360f)
				{
					this.material.SetFloat("_Value3", z);
				}
				if (z < 0f)
				{
					this.material.SetFloat("_Value3", z);
				}
			}
			else
			{
				this.material.SetFloat("_Value3", this.DirectionX);
			}
			this.material.SetFloat("_Value4", this.Speed);
			this.material.SetFloat("_Value5", this.Size);
			this.material.SetFloat("_Value6", this.Distortion);
			this.material.SetFloat("_Value7", this.StormFlashOnOff);
			this.material.SetFloat("_Value8", this.DropOnOff);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("Drop_Near", this.Drop_Near);
			this.material.SetFloat("Drop_Far", this.Drop_Far);
			this.material.SetFloat("Drop_With_Obj", 1f - this.Drop_With_Obj);
			this.material.SetFloat("Myst", this.Myst);
			this.material.SetColor("Myst_Color", this.Myst_Color);
			this.material.SetFloat("Drop_Floor_Fluid", this.Drop_Floor_Fluid);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000633C File Offset: 0x0000453C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000145 RID: 325
	public Shader SCShader;

	// Token: 0x04000146 RID: 326
	private float TimeX = 1f;

	// Token: 0x04000147 RID: 327
	private Vector4 ScreenResolution;

	// Token: 0x04000148 RID: 328
	private Material SCMaterial;

	// Token: 0x04000149 RID: 329
	[Range(0f, 100f)]
	public float _FixDistance = 3f;

	// Token: 0x0400014A RID: 330
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400014B RID: 331
	[Range(0f, 2f)]
	public float Intensity = 0.5f;

	// Token: 0x0400014C RID: 332
	public bool DirectionFollowCameraZ;

	// Token: 0x0400014D RID: 333
	[Range(-0.45f, 0.45f)]
	public float DirectionX = 0.12f;

	// Token: 0x0400014E RID: 334
	[Range(0.4f, 2f)]
	public float Size = 1.5f;

	// Token: 0x0400014F RID: 335
	[Range(0f, 0.5f)]
	public float Speed = 0.275f;

	// Token: 0x04000150 RID: 336
	[Range(0f, 0.5f)]
	public float Distortion = 0.025f;

	// Token: 0x04000151 RID: 337
	[Range(0f, 1f)]
	public float StormFlashOnOff = 1f;

	// Token: 0x04000152 RID: 338
	[Range(0f, 1f)]
	public float DropOnOff = 1f;

	// Token: 0x04000153 RID: 339
	[Range(-0.5f, 0.99f)]
	public float Drop_Near;

	// Token: 0x04000154 RID: 340
	[Range(0f, 1f)]
	public float Drop_Far = 0.5f;

	// Token: 0x04000155 RID: 341
	[Range(0f, 1f)]
	public float Drop_With_Obj = 0.2f;

	// Token: 0x04000156 RID: 342
	[Range(0f, 1f)]
	public float Myst = 0.1f;

	// Token: 0x04000157 RID: 343
	[Range(0f, 1f)]
	public float Drop_Floor_Fluid;

	// Token: 0x04000158 RID: 344
	public Color Myst_Color = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04000159 RID: 345
	private Texture2D Texture2;
}
