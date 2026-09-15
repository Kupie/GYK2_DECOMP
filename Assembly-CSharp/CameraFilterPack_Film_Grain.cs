using System;
using UnityEngine;

// Token: 0x020000A0 RID: 160
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Film/Grain")]
public class CameraFilterPack_Film_Grain : MonoBehaviour
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00014105 File Offset: 0x00012305
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

	// Token: 0x060003E9 RID: 1001 RVA: 0x00014139 File Offset: 0x00012339
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Film_Grain");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001415C File Offset: 0x0001235C
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x00014212 File Offset: 0x00012412
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000559 RID: 1369
	public Shader SCShader;

	// Token: 0x0400055A RID: 1370
	private float TimeX = 1f;

	// Token: 0x0400055B RID: 1371
	private Vector4 ScreenResolution;

	// Token: 0x0400055C RID: 1372
	private Material SCMaterial;

	// Token: 0x0400055D RID: 1373
	[Range(-64f, 64f)]
	public float Value = 32f;
}
