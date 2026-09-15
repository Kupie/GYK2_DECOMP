using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Wave_Horizontal")]
public class CameraFilterPack_Distortion_Wave_Horizontal : MonoBehaviour
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000309 RID: 777 RVA: 0x00010A60 File Offset: 0x0000EC60
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

	// Token: 0x0600030A RID: 778 RVA: 0x00010A94 File Offset: 0x0000EC94
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Wave_Horizontal");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00010AB8 File Offset: 0x0000ECB8
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_WaveIntensity", this.WaveIntensity);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00010B67 File Offset: 0x0000ED67
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400045F RID: 1119
	public Shader SCShader;

	// Token: 0x04000460 RID: 1120
	private float TimeX = 1f;

	// Token: 0x04000461 RID: 1121
	private Vector4 ScreenResolution;

	// Token: 0x04000462 RID: 1122
	private Material SCMaterial;

	// Token: 0x04000463 RID: 1123
	[Range(1f, 100f)]
	public float WaveIntensity = 32f;
}
