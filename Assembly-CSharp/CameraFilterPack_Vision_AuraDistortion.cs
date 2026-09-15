using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/AuraDistortion")]
public class CameraFilterPack_Vision_AuraDistortion : MonoBehaviour
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001F0CC File Offset: 0x0001D2CC
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

	// Token: 0x06000682 RID: 1666 RVA: 0x0001F100 File Offset: 0x0001D300
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_AuraDistortion");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0001F124 File Offset: 0x0001D324
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
			this.material.SetFloat("_Value", this.Twist);
			this.material.SetColor("_Value2", this.Color);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Value5", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0001F232 File Offset: 0x0001D432
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400084F RID: 2127
	public Shader SCShader;

	// Token: 0x04000850 RID: 2128
	private float TimeX = 1f;

	// Token: 0x04000851 RID: 2129
	private Vector4 ScreenResolution;

	// Token: 0x04000852 RID: 2130
	private Material SCMaterial;

	// Token: 0x04000853 RID: 2131
	[Range(0f, 2f)]
	public float Twist = 1f;

	// Token: 0x04000854 RID: 2132
	[Range(-4f, 4f)]
	public float Speed = 1f;

	// Token: 0x04000855 RID: 2133
	public Color Color = new Color(0.16f, 0.57f, 0.19f);

	// Token: 0x04000856 RID: 2134
	[Range(-1f, 2f)]
	public float PosX = 0.5f;

	// Token: 0x04000857 RID: 2135
	[Range(-1f, 2f)]
	public float PosY = 0.5f;
}
