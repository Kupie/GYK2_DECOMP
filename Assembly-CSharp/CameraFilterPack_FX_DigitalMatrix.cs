using System;
using UnityEngine;

// Token: 0x020000A6 RID: 166
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DigitalMatrix")]
public class CameraFilterPack_FX_DigitalMatrix : MonoBehaviour
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x00014A1C File Offset: 0x00012C1C
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

	// Token: 0x0600040D RID: 1037 RVA: 0x00014A50 File Offset: 0x00012C50
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_DigitalMatrix");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00014A74 File Offset: 0x00012C74
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
			this.material.SetFloat("_Value2", this.ColorR);
			this.material.SetFloat("_Value3", this.ColorG);
			this.material.SetFloat("_Value4", this.ColorB);
			this.material.SetFloat("_Value5", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00014B82 File Offset: 0x00012D82
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000581 RID: 1409
	public Shader SCShader;

	// Token: 0x04000582 RID: 1410
	private float TimeX = 1f;

	// Token: 0x04000583 RID: 1411
	private Vector4 ScreenResolution;

	// Token: 0x04000584 RID: 1412
	private Material SCMaterial;

	// Token: 0x04000585 RID: 1413
	[Range(0.4f, 5f)]
	public float Size = 1f;

	// Token: 0x04000586 RID: 1414
	[Range(-10f, 10f)]
	public float Speed = 1f;

	// Token: 0x04000587 RID: 1415
	[Range(-1f, 1f)]
	public float ColorR = -1f;

	// Token: 0x04000588 RID: 1416
	[Range(-1f, 1f)]
	public float ColorG = 1f;

	// Token: 0x04000589 RID: 1417
	[Range(-1f, 1f)]
	public float ColorB = -1f;
}
