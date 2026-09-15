using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DigitalMatrixDistortion")]
public class CameraFilterPack_FX_DigitalMatrixDistortion : MonoBehaviour
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x00014BF1 File Offset: 0x00012DF1
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

	// Token: 0x06000413 RID: 1043 RVA: 0x00014C25 File Offset: 0x00012E25
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_DigitalMatrixDistortion");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00014C48 File Offset: 0x00012E48
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
			this.material.SetFloat("_Value2", this.Distortion);
			this.material.SetFloat("_Value5", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00014D2A File Offset: 0x00012F2A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400058A RID: 1418
	public Shader SCShader;

	// Token: 0x0400058B RID: 1419
	private float TimeX = 1f;

	// Token: 0x0400058C RID: 1420
	private Vector4 ScreenResolution;

	// Token: 0x0400058D RID: 1421
	private Material SCMaterial;

	// Token: 0x0400058E RID: 1422
	[Range(0.4f, 5f)]
	public float Size = 1.4f;

	// Token: 0x0400058F RID: 1423
	[Range(-2f, 2f)]
	public float Speed = 0.5f;

	// Token: 0x04000590 RID: 1424
	[Range(-5f, 5f)]
	public float Distortion = 2.3f;
}
