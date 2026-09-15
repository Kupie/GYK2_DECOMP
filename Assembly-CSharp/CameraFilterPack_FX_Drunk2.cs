using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Drunk2")]
public class CameraFilterPack_FX_Drunk2 : MonoBehaviour
{
	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000424 RID: 1060 RVA: 0x00015115 File Offset: 0x00013315
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

	// Token: 0x06000425 RID: 1061 RVA: 0x00015149 File Offset: 0x00013349
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Drunk2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001516C File Offset: 0x0001336C
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
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00015264 File Offset: 0x00013464
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005A5 RID: 1445
	public Shader SCShader;

	// Token: 0x040005A6 RID: 1446
	private float TimeX = 1f;

	// Token: 0x040005A7 RID: 1447
	private Vector4 ScreenResolution;

	// Token: 0x040005A8 RID: 1448
	private Material SCMaterial;

	// Token: 0x040005A9 RID: 1449
	[Range(0f, 10f)]
	private float Value = 1f;

	// Token: 0x040005AA RID: 1450
	[Range(0f, 10f)]
	private float Value2 = 1f;

	// Token: 0x040005AB RID: 1451
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x040005AC RID: 1452
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
