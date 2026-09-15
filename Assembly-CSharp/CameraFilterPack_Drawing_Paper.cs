using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Paper")]
public class CameraFilterPack_Drawing_Paper : MonoBehaviour
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000387 RID: 903 RVA: 0x0001281B File Offset: 0x00010A1B
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

	// Token: 0x06000388 RID: 904 RVA: 0x0001284F File Offset: 0x00010A4F
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Paper1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Paper");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00012888 File Offset: 0x00010A88
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
			this.material.SetColor("_PColor", this.Pencil_Color);
			this.material.SetFloat("_Value1", this.Pencil_Size);
			this.material.SetFloat("_Value2", this.Pencil_Correction);
			this.material.SetFloat("_Value3", this.Intensity);
			this.material.SetFloat("_Value4", this.Speed_Animation);
			this.material.SetFloat("_Value5", this.Corner_Lose);
			this.material.SetFloat("_Value6", this.Fade_Paper_to_BackColor);
			this.material.SetFloat("_Value7", this.Fade_With_Original);
			this.material.SetColor("_PColor2", this.Back_Color);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600038B RID: 907 RVA: 0x000129D7 File Offset: 0x00010BD7
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004E2 RID: 1250
	public Shader SCShader;

	// Token: 0x040004E3 RID: 1251
	private float TimeX = 1f;

	// Token: 0x040004E4 RID: 1252
	public Color Pencil_Color = new Color(0.156f, 0.3f, 0.738f, 1f);

	// Token: 0x040004E5 RID: 1253
	[Range(0.0001f, 0.0022f)]
	public float Pencil_Size = 0.0008f;

	// Token: 0x040004E6 RID: 1254
	[Range(0f, 2f)]
	public float Pencil_Correction = 0.76f;

	// Token: 0x040004E7 RID: 1255
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x040004E8 RID: 1256
	[Range(0f, 2f)]
	public float Speed_Animation = 1f;

	// Token: 0x040004E9 RID: 1257
	[Range(0f, 1f)]
	public float Corner_Lose = 0.5f;

	// Token: 0x040004EA RID: 1258
	[Range(0f, 1f)]
	public float Fade_Paper_to_BackColor;

	// Token: 0x040004EB RID: 1259
	[Range(0f, 1f)]
	public float Fade_With_Original = 1f;

	// Token: 0x040004EC RID: 1260
	public Color Back_Color = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040004ED RID: 1261
	private Material SCMaterial;

	// Token: 0x040004EE RID: 1262
	private Texture2D Texture2;
}
