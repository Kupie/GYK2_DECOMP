using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Blood_Plus")]
public class CameraFilterPack_AAA_Blood_Plus : MonoBehaviour
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600006A RID: 106 RVA: 0x00004BBE File Offset: 0x00002DBE
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

	// Token: 0x0600006B RID: 107 RVA: 0x00004BF2 File Offset: 0x00002DF2
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_AAA_Blood2") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Blood_Plus");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00004C28 File Offset: 0x00002E28
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
			this.material.SetFloat("_Value", this.LightReflect);
			this.material.SetFloat("_Value2", Mathf.Clamp(this.Blood_1, 0f, 1f));
			this.material.SetFloat("_Value3", Mathf.Clamp(this.Blood_2, 0f, 1f));
			this.material.SetFloat("_Value4", Mathf.Clamp(this.Blood_3, 0f, 1f));
			this.material.SetFloat("_Value5", Mathf.Clamp(this.Blood_4, 0f, 1f));
			this.material.SetFloat("_Value6", Mathf.Clamp(this.Blood_5, 0f, 1f));
			this.material.SetFloat("_Value7", Mathf.Clamp(this.Blood_6, 0f, 1f));
			this.material.SetFloat("_Value8", Mathf.Clamp(this.Blood_7, 0f, 1f));
			this.material.SetFloat("_Value9", Mathf.Clamp(this.Blood_8, 0f, 1f));
			this.material.SetFloat("_Value10", Mathf.Clamp(this.Blood_9, 0f, 1f));
			this.material.SetFloat("_Value11", Mathf.Clamp(this.Blood_10, 0f, 1f));
			this.material.SetFloat("_Value12", Mathf.Clamp(this.Blood_11, 0f, 1f));
			this.material.SetFloat("_Value13", Mathf.Clamp(this.Blood_12, 0f, 1f));
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004E83 File Offset: 0x00003083
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000DD RID: 221
	public Shader SCShader;

	// Token: 0x040000DE RID: 222
	private float TimeX = 1f;

	// Token: 0x040000DF RID: 223
	[Range(0f, 1f)]
	public float Blood_1 = 1f;

	// Token: 0x040000E0 RID: 224
	[Range(0f, 1f)]
	public float Blood_2;

	// Token: 0x040000E1 RID: 225
	[Range(0f, 1f)]
	public float Blood_3;

	// Token: 0x040000E2 RID: 226
	[Range(0f, 1f)]
	public float Blood_4;

	// Token: 0x040000E3 RID: 227
	[Range(0f, 1f)]
	public float Blood_5;

	// Token: 0x040000E4 RID: 228
	[Range(0f, 1f)]
	public float Blood_6;

	// Token: 0x040000E5 RID: 229
	[Range(0f, 1f)]
	public float Blood_7;

	// Token: 0x040000E6 RID: 230
	[Range(0f, 1f)]
	public float Blood_8;

	// Token: 0x040000E7 RID: 231
	[Range(0f, 1f)]
	public float Blood_9;

	// Token: 0x040000E8 RID: 232
	[Range(0f, 1f)]
	public float Blood_10;

	// Token: 0x040000E9 RID: 233
	[Range(0f, 1f)]
	public float Blood_11;

	// Token: 0x040000EA RID: 234
	[Range(0f, 1f)]
	public float Blood_12;

	// Token: 0x040000EB RID: 235
	[Range(0f, 1f)]
	public float LightReflect = 0.5f;

	// Token: 0x040000EC RID: 236
	private Material SCMaterial;

	// Token: 0x040000ED RID: 237
	private Texture2D Texture2;
}
