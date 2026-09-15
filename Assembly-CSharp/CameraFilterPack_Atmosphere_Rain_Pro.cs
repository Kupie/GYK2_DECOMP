using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Rain_Pro")]
public class CameraFilterPack_Atmosphere_Rain_Pro : MonoBehaviour
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000A0 RID: 160 RVA: 0x00005E03 File Offset: 0x00004003
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

	// Token: 0x060000A1 RID: 161 RVA: 0x00005E37 File Offset: 0x00004037
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Rain_Pro");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005E70 File Offset: 0x00004070
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
			this.material.SetFloat("_Value3", this.DirectionX);
			this.material.SetFloat("_Value4", this.Speed);
			this.material.SetFloat("_Value5", this.Size);
			this.material.SetFloat("_Value6", this.Distortion);
			this.material.SetFloat("_Value7", this.StormFlashOnOff);
			this.material.SetFloat("_Value8", this.DropOnOff);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005FD6 File Offset: 0x000041D6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000138 RID: 312
	public Shader SCShader;

	// Token: 0x04000139 RID: 313
	private float TimeX = 1f;

	// Token: 0x0400013A RID: 314
	private Vector4 ScreenResolution;

	// Token: 0x0400013B RID: 315
	private Material SCMaterial;

	// Token: 0x0400013C RID: 316
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400013D RID: 317
	[Range(0f, 2f)]
	public float Intensity = 0.5f;

	// Token: 0x0400013E RID: 318
	[Range(-0.25f, 0.25f)]
	public float DirectionX = 0.12f;

	// Token: 0x0400013F RID: 319
	[Range(0.4f, 2f)]
	public float Size = 1.5f;

	// Token: 0x04000140 RID: 320
	[Range(0f, 0.5f)]
	public float Speed = 0.275f;

	// Token: 0x04000141 RID: 321
	[Range(0f, 0.5f)]
	public float Distortion = 0.025f;

	// Token: 0x04000142 RID: 322
	[Range(0f, 1f)]
	public float StormFlashOnOff = 1f;

	// Token: 0x04000143 RID: 323
	[Range(0f, 1f)]
	public float DropOnOff = 1f;

	// Token: 0x04000144 RID: 324
	private Texture2D Texture2;
}
