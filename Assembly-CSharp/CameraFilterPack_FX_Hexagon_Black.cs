using System;
using UnityEngine;

// Token: 0x020000B2 RID: 178
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Hexagon_Black")]
public class CameraFilterPack_FX_Hexagon_Black : MonoBehaviour
{
	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000454 RID: 1108 RVA: 0x00015BA6 File Offset: 0x00013DA6
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

	// Token: 0x06000455 RID: 1109 RVA: 0x00015BDA File Offset: 0x00013DDA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Hexagon_Black");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00015BFC File Offset: 0x00013DFC
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00015CB2 File Offset: 0x00013EB2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005D2 RID: 1490
	public Shader SCShader;

	// Token: 0x040005D3 RID: 1491
	private float TimeX = 1f;

	// Token: 0x040005D4 RID: 1492
	private Vector4 ScreenResolution;

	// Token: 0x040005D5 RID: 1493
	private Material SCMaterial;

	// Token: 0x040005D6 RID: 1494
	[Range(0.2f, 10f)]
	public float Value = 1f;
}
