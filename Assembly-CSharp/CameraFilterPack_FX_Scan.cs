using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Scan")]
public class CameraFilterPack_FX_Scan : MonoBehaviour
{
	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001631E File Offset: 0x0001451E
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

	// Token: 0x06000479 RID: 1145 RVA: 0x00016352 File Offset: 0x00014552
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Scan");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00016374 File Offset: 0x00014574
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
			this.material.SetFloat("_Value2", this.Speed);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001646C File Offset: 0x0001466C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005F0 RID: 1520
	public Shader SCShader;

	// Token: 0x040005F1 RID: 1521
	private float TimeX = 1f;

	// Token: 0x040005F2 RID: 1522
	private Vector4 ScreenResolution;

	// Token: 0x040005F3 RID: 1523
	private Material SCMaterial;

	// Token: 0x040005F4 RID: 1524
	[Range(0.001f, 0.1f)]
	public float Size = 0.025f;

	// Token: 0x040005F5 RID: 1525
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x040005F6 RID: 1526
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x040005F7 RID: 1527
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
