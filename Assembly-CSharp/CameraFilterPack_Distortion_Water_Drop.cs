using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Water_Drop")]
public class CameraFilterPack_Distortion_Water_Drop : MonoBehaviour
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000303 RID: 771 RVA: 0x000108DA File Offset: 0x0000EADA
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

	// Token: 0x06000304 RID: 772 RVA: 0x0001090E File Offset: 0x0000EB0E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Water_Drop");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00010930 File Offset: 0x0000EB30
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
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			this.material.SetFloat("_WaveIntensity", this.WaveIntensity);
			this.material.SetInt("_NumberOfWaves", this.NumberOfWaves);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00010A21 File Offset: 0x0000EC21
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000457 RID: 1111
	public Shader SCShader;

	// Token: 0x04000458 RID: 1112
	private float TimeX = 1f;

	// Token: 0x04000459 RID: 1113
	private Vector4 ScreenResolution;

	// Token: 0x0400045A RID: 1114
	private Material SCMaterial;

	// Token: 0x0400045B RID: 1115
	[Range(-1f, 1f)]
	public float CenterX;

	// Token: 0x0400045C RID: 1116
	[Range(-1f, 1f)]
	public float CenterY;

	// Token: 0x0400045D RID: 1117
	[Range(0f, 10f)]
	public float WaveIntensity = 1f;

	// Token: 0x0400045E RID: 1118
	[Range(0f, 20f)]
	public int NumberOfWaves = 5;
}
