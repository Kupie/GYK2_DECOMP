using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/CellShading2")]
public class CameraFilterPack_Drawing_CellShading2 : MonoBehaviour
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600031B RID: 795 RVA: 0x00010F80 File Offset: 0x0000F180
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

	// Token: 0x0600031C RID: 796 RVA: 0x00010FB4 File Offset: 0x0000F1B4
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00010FD8 File Offset: 0x0000F1D8
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
			this.material.SetFloat("_EdgeSize", this.EdgeSize);
			this.material.SetFloat("_ColorLevel", this.ColorLevel);
			this.material.SetFloat("_Distortion", this.Blur);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000110B3 File Offset: 0x0000F2B3
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000477 RID: 1143
	public Shader SCShader;

	// Token: 0x04000478 RID: 1144
	private float TimeX = 1f;

	// Token: 0x04000479 RID: 1145
	private Vector4 ScreenResolution;

	// Token: 0x0400047A RID: 1146
	private Material SCMaterial;

	// Token: 0x0400047B RID: 1147
	[Range(0f, 1f)]
	public float EdgeSize = 0.1f;

	// Token: 0x0400047C RID: 1148
	[Range(0f, 10f)]
	public float ColorLevel = 4f;

	// Token: 0x0400047D RID: 1149
	[Range(0f, 1f)]
	public float Blur = 1f;
}
