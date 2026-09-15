using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Crystal")]
public class CameraFilterPack_Vision_Crystal : MonoBehaviour
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001F601 File Offset: 0x0001D801
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

	// Token: 0x06000694 RID: 1684 RVA: 0x0001F635 File Offset: 0x0001D835
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Crystal");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0001F658 File Offset: 0x0001D858
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
			this.material.SetFloat("_Value2", this.X);
			this.material.SetFloat("_Value3", this.Y);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0001F750 File Offset: 0x0001D950
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000868 RID: 2152
	public Shader SCShader;

	// Token: 0x04000869 RID: 2153
	private float TimeX = 1f;

	// Token: 0x0400086A RID: 2154
	private Vector4 ScreenResolution;

	// Token: 0x0400086B RID: 2155
	private Material SCMaterial;

	// Token: 0x0400086C RID: 2156
	[Range(-10f, 10f)]
	public float Value = 1f;

	// Token: 0x0400086D RID: 2157
	[Range(-1f, 1f)]
	public float X = 1f;

	// Token: 0x0400086E RID: 2158
	[Range(-1f, 1f)]
	public float Y = 1f;

	// Token: 0x0400086F RID: 2159
	[Range(-1f, 1f)]
	private float Value4 = 1f;
}
