using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Paper3")]
public class CameraFilterPack_Drawing_Paper3 : MonoBehaviour
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000393 RID: 915 RVA: 0x00012D06 File Offset: 0x00010F06
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

	// Token: 0x06000394 RID: 916 RVA: 0x00012D3A File Offset: 0x00010F3A
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Paper4") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Paper3");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00012D70 File Offset: 0x00010F70
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

	// Token: 0x06000396 RID: 918 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00012EBF File Offset: 0x000110BF
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004FC RID: 1276
	public Shader SCShader;

	// Token: 0x040004FD RID: 1277
	private float TimeX = 1f;

	// Token: 0x040004FE RID: 1278
	public Color Pencil_Color = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040004FF RID: 1279
	[Range(0.0001f, 0.0022f)]
	public float Pencil_Size = 0.00125f;

	// Token: 0x04000500 RID: 1280
	[Range(0f, 2f)]
	public float Pencil_Correction = 0.35f;

	// Token: 0x04000501 RID: 1281
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x04000502 RID: 1282
	[Range(0f, 2f)]
	public float Speed_Animation = 1f;

	// Token: 0x04000503 RID: 1283
	[Range(0f, 1f)]
	public float Corner_Lose = 1f;

	// Token: 0x04000504 RID: 1284
	[Range(0f, 1f)]
	public float Fade_Paper_to_BackColor;

	// Token: 0x04000505 RID: 1285
	[Range(0f, 1f)]
	public float Fade_With_Original = 1f;

	// Token: 0x04000506 RID: 1286
	public Color Back_Color = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04000507 RID: 1287
	private Material SCMaterial;

	// Token: 0x04000508 RID: 1288
	private Texture2D Texture2;
}
