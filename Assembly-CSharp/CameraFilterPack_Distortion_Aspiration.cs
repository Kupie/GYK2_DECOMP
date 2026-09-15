using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Aspiration")]
public class CameraFilterPack_Distortion_Aspiration : MonoBehaviour
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600029D RID: 669 RVA: 0x0000F057 File Offset: 0x0000D257
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

	// Token: 0x0600029E RID: 670 RVA: 0x0000F08B File Offset: 0x0000D28B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Aspiration");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000F0AC File Offset: 0x0000D2AC
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
			this.material.SetFloat("_Value", 1f - this.Value);
			this.material.SetFloat("_Value2", this.PosX);
			this.material.SetFloat("_Value3", this.PosY);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000F1AA File Offset: 0x0000D3AA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003E6 RID: 998
	public Shader SCShader;

	// Token: 0x040003E7 RID: 999
	private float TimeX = 1f;

	// Token: 0x040003E8 RID: 1000
	private Vector4 ScreenResolution;

	// Token: 0x040003E9 RID: 1001
	private Material SCMaterial;

	// Token: 0x040003EA RID: 1002
	[Range(0f, 1f)]
	public float Value = 0.8f;

	// Token: 0x040003EB RID: 1003
	[Range(-1f, 1f)]
	public float PosX = 0.5f;

	// Token: 0x040003EC RID: 1004
	[Range(-1f, 1f)]
	public float PosY = 0.5f;

	// Token: 0x040003ED RID: 1005
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
