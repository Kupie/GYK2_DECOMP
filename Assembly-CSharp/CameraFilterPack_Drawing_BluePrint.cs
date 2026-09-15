using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/BluePrint")]
public class CameraFilterPack_Drawing_BluePrint : MonoBehaviour
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600030F RID: 783 RVA: 0x00010B9F File Offset: 0x0000ED9F
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

	// Token: 0x06000310 RID: 784 RVA: 0x00010BD3 File Offset: 0x0000EDD3
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Paper2") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_BluePrint");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00010C0C File Offset: 0x0000EE0C
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

	// Token: 0x06000312 RID: 786 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00010D5B File Offset: 0x0000EF5B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000464 RID: 1124
	public Shader SCShader;

	// Token: 0x04000465 RID: 1125
	private float TimeX = 1f;

	// Token: 0x04000466 RID: 1126
	public Color Pencil_Color = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04000467 RID: 1127
	[Range(0.0001f, 0.0022f)]
	public float Pencil_Size = 0.0008f;

	// Token: 0x04000468 RID: 1128
	[Range(0f, 2f)]
	public float Pencil_Correction = 0.76f;

	// Token: 0x04000469 RID: 1129
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x0400046A RID: 1130
	[Range(0f, 2f)]
	public float Speed_Animation = 1f;

	// Token: 0x0400046B RID: 1131
	[Range(0f, 1f)]
	public float Corner_Lose = 0.5f;

	// Token: 0x0400046C RID: 1132
	[Range(0f, 1f)]
	public float Fade_Paper_to_BackColor = 0.2f;

	// Token: 0x0400046D RID: 1133
	[Range(0f, 1f)]
	public float Fade_With_Original = 1f;

	// Token: 0x0400046E RID: 1134
	public Color Back_Color = new Color(0.175f, 0.402f, 0.687f, 1f);

	// Token: 0x0400046F RID: 1135
	private Material SCMaterial;

	// Token: 0x04000470 RID: 1136
	private Texture2D Texture2;
}
