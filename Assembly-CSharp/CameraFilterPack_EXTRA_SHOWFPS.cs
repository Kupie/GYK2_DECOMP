using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009B RID: 155
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/EXTRA/SHOWFPS")]
public class CameraFilterPack_EXTRA_SHOWFPS : MonoBehaviour
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060003C9 RID: 969 RVA: 0x0001397B File Offset: 0x00011B7B
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

	// Token: 0x060003CA RID: 970 RVA: 0x000139AF File Offset: 0x00011BAF
	private void Start()
	{
		this.FPS = 0;
		base.StartCoroutine(this.FPSX());
		this.SCShader = Shader.Find("CameraFilterPack/EXTRA_SHOWFPS");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x000139E4 File Offset: 0x00011BE4
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", (float)this.FPS);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00013ADD File Offset: 0x00011CDD
	private IEnumerator FPSX()
	{
		for (;;)
		{
			float num = this.accum / (float)this.frames;
			this.FPS = (int)num;
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00013AEC File Offset: 0x00011CEC
	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00013B14 File Offset: 0x00011D14
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000533 RID: 1331
	public Shader SCShader;

	// Token: 0x04000534 RID: 1332
	private float TimeX = 1f;

	// Token: 0x04000535 RID: 1333
	private Vector4 ScreenResolution;

	// Token: 0x04000536 RID: 1334
	private Material SCMaterial;

	// Token: 0x04000537 RID: 1335
	[Range(8f, 42f)]
	public float Size = 12f;

	// Token: 0x04000538 RID: 1336
	[Range(0f, 100f)]
	private int FPS = 1;

	// Token: 0x04000539 RID: 1337
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x0400053A RID: 1338
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x0400053B RID: 1339
	private float accum;

	// Token: 0x0400053C RID: 1340
	private int frames;

	// Token: 0x0400053D RID: 1341
	public float frequency = 0.5f;
}
