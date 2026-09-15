using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Rain")]
public class CameraFilterPack_Atmosphere_Rain : MonoBehaviour
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600009A RID: 154 RVA: 0x00005BBF File Offset: 0x00003DBF
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

	// Token: 0x0600009B RID: 155 RVA: 0x00005BF3 File Offset: 0x00003DF3
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Rain");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005C2C File Offset: 0x00003E2C
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00005D7C File Offset: 0x00003F7C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400012C RID: 300
	public Shader SCShader;

	// Token: 0x0400012D RID: 301
	private float TimeX = 1f;

	// Token: 0x0400012E RID: 302
	private Vector4 ScreenResolution;

	// Token: 0x0400012F RID: 303
	private Material SCMaterial;

	// Token: 0x04000130 RID: 304
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000131 RID: 305
	[Range(0f, 2f)]
	public float Intensity = 0.5f;

	// Token: 0x04000132 RID: 306
	[Range(-0.25f, 0.25f)]
	public float DirectionX = 0.12f;

	// Token: 0x04000133 RID: 307
	[Range(0.4f, 2f)]
	public float Size = 1.5f;

	// Token: 0x04000134 RID: 308
	[Range(0f, 0.5f)]
	public float Speed = 0.275f;

	// Token: 0x04000135 RID: 309
	[Range(0f, 0.5f)]
	public float Distortion = 0.05f;

	// Token: 0x04000136 RID: 310
	[Range(0f, 1f)]
	public float StormFlashOnOff = 1f;

	// Token: 0x04000137 RID: 311
	private Texture2D Texture2;
}
