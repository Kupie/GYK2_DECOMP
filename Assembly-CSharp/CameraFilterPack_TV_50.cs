using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/50s")]
public class CameraFilterPack_TV_50 : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001BB31 File Offset: 0x00019D31
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

	// Token: 0x060005AA RID: 1450 RVA: 0x0001BB65 File Offset: 0x00019D65
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_50");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0001BB88 File Offset: 0x00019D88
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
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001BC3E File Offset: 0x00019E3E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000765 RID: 1893
	public Shader SCShader;

	// Token: 0x04000766 RID: 1894
	private float TimeX = 1f;

	// Token: 0x04000767 RID: 1895
	private Vector4 ScreenResolution;

	// Token: 0x04000768 RID: 1896
	private Material SCMaterial;

	// Token: 0x04000769 RID: 1897
	[Range(0f, 1f)]
	public float Fade = 1f;
}
