using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Light/Water2")]
public class CameraFilterPack_Light_Water2 : MonoBehaviour
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060004F6 RID: 1270 RVA: 0x000183BE File Offset: 0x000165BE
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

	// Token: 0x060004F7 RID: 1271 RVA: 0x000183F2 File Offset: 0x000165F2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Light_Water2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00018414 File Offset: 0x00016614
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.Speed_X);
			this.material.SetFloat("_Value3", this.Speed_Y);
			this.material.SetFloat("_Value4", this.Intensity);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001850C File Offset: 0x0001670C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000682 RID: 1666
	public Shader SCShader;

	// Token: 0x04000683 RID: 1667
	private float TimeX = 1f;

	// Token: 0x04000684 RID: 1668
	private Vector4 ScreenResolution;

	// Token: 0x04000685 RID: 1669
	private Material SCMaterial;

	// Token: 0x04000686 RID: 1670
	[Range(0f, 10f)]
	public float Speed = 0.2f;

	// Token: 0x04000687 RID: 1671
	[Range(0f, 10f)]
	public float Speed_X = 0.2f;

	// Token: 0x04000688 RID: 1672
	[Range(0f, 1f)]
	public float Speed_Y = 0.3f;

	// Token: 0x04000689 RID: 1673
	[Range(0f, 10f)]
	public float Intensity = 2.4f;
}
