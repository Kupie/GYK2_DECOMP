using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/BlackLine")]
public class CameraFilterPack_Edge_BlackLine : MonoBehaviour
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600039F RID: 927 RVA: 0x000130AF File Offset: 0x000112AF
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

	// Token: 0x060003A0 RID: 928 RVA: 0x000130E3 File Offset: 0x000112E3
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_BlackLine");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00013104 File Offset: 0x00011304
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
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001319A File Offset: 0x0001139A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400050E RID: 1294
	public Shader SCShader;

	// Token: 0x0400050F RID: 1295
	private float TimeX = 1f;

	// Token: 0x04000510 RID: 1296
	private Vector4 ScreenResolution;

	// Token: 0x04000511 RID: 1297
	private Material SCMaterial;
}
