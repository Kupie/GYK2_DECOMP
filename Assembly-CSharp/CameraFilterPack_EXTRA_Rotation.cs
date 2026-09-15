using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/EXTRA/Rotation")]
public class CameraFilterPack_EXTRA_Rotation : MonoBehaviour
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x000137DE File Offset: 0x000119DE
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

	// Token: 0x060003C4 RID: 964 RVA: 0x00013812 File Offset: 0x00011A12
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/EXTRA_Rotation");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00013834 File Offset: 0x00011A34
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
			this.material.SetFloat("_Value", -this.Rotation);
			this.material.SetFloat("_Value2", this.PositionX);
			this.material.SetFloat("_Value3", this.PositionY);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001392D File Offset: 0x00011B2D
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400052B RID: 1323
	public Shader SCShader;

	// Token: 0x0400052C RID: 1324
	private float TimeX = 1f;

	// Token: 0x0400052D RID: 1325
	private Vector4 ScreenResolution;

	// Token: 0x0400052E RID: 1326
	private Material SCMaterial;

	// Token: 0x0400052F RID: 1327
	[Range(-360f, 360f)]
	public float Rotation;

	// Token: 0x04000530 RID: 1328
	[Range(-1f, 2f)]
	public float PositionX = 0.5f;

	// Token: 0x04000531 RID: 1329
	[Range(-1f, 2f)]
	public float PositionY = 0.5f;

	// Token: 0x04000532 RID: 1330
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
