using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Dream")]
public class CameraFilterPack_Distortion_Dream : MonoBehaviour
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000F699 File Offset: 0x0000D899
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

	// Token: 0x060002B6 RID: 694 RVA: 0x0000F6CD File Offset: 0x0000D8CD
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Dream");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
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
			this.material.SetFloat("_Distortion", this.Distortion);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000F776 File Offset: 0x0000D976
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000404 RID: 1028
	public Shader SCShader;

	// Token: 0x04000405 RID: 1029
	private float TimeX = 1f;

	// Token: 0x04000406 RID: 1030
	[Range(1f, 10f)]
	public float Distortion = 1f;

	// Token: 0x04000407 RID: 1031
	private Material SCMaterial;
}
