using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Drunk")]
public class CameraFilterPack_FX_Drunk : MonoBehaviour
{
	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x00014EBE File Offset: 0x000130BE
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

	// Token: 0x0600041F RID: 1055 RVA: 0x00014EF2 File Offset: 0x000130F2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Drunk");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00014F14 File Offset: 0x00013114
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetFloat("_DistortionWave", this.DistortionWave);
			this.material.SetFloat("_Wavy", this.Wavy);
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetFloat("_ColoredChange", this.ColoredChange);
			this.material.SetFloat("_ChangeRed", this.ChangeRed);
			this.material.SetFloat("_ChangeGreen", this.ChangeGreen);
			this.material.SetFloat("_ChangeBlue", this.ChangeBlue);
			this.material.SetFloat("_Colored", this.ColoredSaturate);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000150A6 File Offset: 0x000132A6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000596 RID: 1430
	public Shader SCShader;

	// Token: 0x04000597 RID: 1431
	private float TimeX = 1f;

	// Token: 0x04000598 RID: 1432
	private Vector4 ScreenResolution;

	// Token: 0x04000599 RID: 1433
	private Material SCMaterial;

	// Token: 0x0400059A RID: 1434
	[HideInInspector]
	[Range(0f, 20f)]
	public float Value = 6f;

	// Token: 0x0400059B RID: 1435
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x0400059C RID: 1436
	[Range(0f, 1f)]
	public float Wavy = 1f;

	// Token: 0x0400059D RID: 1437
	[Range(0f, 1f)]
	public float Distortion;

	// Token: 0x0400059E RID: 1438
	[Range(0f, 1f)]
	public float DistortionWave;

	// Token: 0x0400059F RID: 1439
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040005A0 RID: 1440
	[Range(-2f, 2f)]
	public float ColoredSaturate = 1f;

	// Token: 0x040005A1 RID: 1441
	[Range(-1f, 2f)]
	public float ColoredChange;

	// Token: 0x040005A2 RID: 1442
	[Range(-1f, 1f)]
	public float ChangeRed;

	// Token: 0x040005A3 RID: 1443
	[Range(-1f, 1f)]
	public float ChangeGreen;

	// Token: 0x040005A4 RID: 1444
	[Range(-1f, 1f)]
	public float ChangeBlue;
}
