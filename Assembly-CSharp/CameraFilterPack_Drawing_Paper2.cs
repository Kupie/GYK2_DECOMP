using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Paper2")]
public class CameraFilterPack_Drawing_Paper2 : MonoBehaviour
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600038D RID: 909 RVA: 0x00012A92 File Offset: 0x00010C92
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

	// Token: 0x0600038E RID: 910 RVA: 0x00012AC6 File Offset: 0x00010CC6
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Paper3") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Paper2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00012AFC File Offset: 0x00010CFC
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

	// Token: 0x06000390 RID: 912 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00012C4B File Offset: 0x00010E4B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004EF RID: 1263
	public Shader SCShader;

	// Token: 0x040004F0 RID: 1264
	private float TimeX = 1f;

	// Token: 0x040004F1 RID: 1265
	public Color Pencil_Color = new Color(0f, 0.371f, 0.78f, 1f);

	// Token: 0x040004F2 RID: 1266
	[Range(0.0001f, 0.0022f)]
	public float Pencil_Size = 0.0008f;

	// Token: 0x040004F3 RID: 1267
	[Range(0f, 2f)]
	public float Pencil_Correction = 0.76f;

	// Token: 0x040004F4 RID: 1268
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x040004F5 RID: 1269
	[Range(0f, 2f)]
	public float Speed_Animation = 1f;

	// Token: 0x040004F6 RID: 1270
	[Range(0f, 1f)]
	public float Corner_Lose = 0.85f;

	// Token: 0x040004F7 RID: 1271
	[Range(0f, 1f)]
	public float Fade_Paper_to_BackColor;

	// Token: 0x040004F8 RID: 1272
	[Range(0f, 1f)]
	public float Fade_With_Original = 1f;

	// Token: 0x040004F9 RID: 1273
	public Color Back_Color = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040004FA RID: 1274
	private Material SCMaterial;

	// Token: 0x040004FB RID: 1275
	private Texture2D Texture2;
}
