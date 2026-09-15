using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Blood")]
public class CameraFilterPack_AAA_Blood : MonoBehaviour
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000058 RID: 88 RVA: 0x000044D1 File Offset: 0x000026D1
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

	// Token: 0x06000059 RID: 89 RVA: 0x00004505 File Offset: 0x00002705
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_AAA_Blood1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Blood");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0000453C File Offset: 0x0000273C
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
			this.material.SetFloat("_Value2", this.Blood1);
			this.material.SetFloat("_Value3", this.Blood2);
			this.material.SetFloat("_Value4", this.Blood3);
			this.material.SetFloat("_Value5", this.Blood4);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00004633 File Offset: 0x00002833
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000B9 RID: 185
	public Shader SCShader;

	// Token: 0x040000BA RID: 186
	private float TimeX = 1f;

	// Token: 0x040000BB RID: 187
	[Range(0f, 128f)]
	public float Blood1;

	// Token: 0x040000BC RID: 188
	[Range(0f, 128f)]
	public float Blood2;

	// Token: 0x040000BD RID: 189
	[Range(0f, 128f)]
	public float Blood3;

	// Token: 0x040000BE RID: 190
	[Range(0f, 128f)]
	public float Blood4 = 1f;

	// Token: 0x040000BF RID: 191
	[Range(0f, 0.004f)]
	public float LightReflect = 0.002f;

	// Token: 0x040000C0 RID: 192
	private Material SCMaterial;

	// Token: 0x040000C1 RID: 193
	private Texture2D Texture2;
}
