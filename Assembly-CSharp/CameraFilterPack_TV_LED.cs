using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/LED")]
public class CameraFilterPack_TV_LED : MonoBehaviour
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001D0F1 File Offset: 0x0001B2F1
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

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001D125 File Offset: 0x0001B325
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_LED");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001D148 File Offset: 0x0001B348
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
			this.material.SetFloat("_Size", (float)this.Size);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001D215 File Offset: 0x0001B415
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007C3 RID: 1987
	public Shader SCShader;

	// Token: 0x040007C4 RID: 1988
	private Vector4 ScreenResolution;

	// Token: 0x040007C5 RID: 1989
	private float TimeX = 1f;

	// Token: 0x040007C6 RID: 1990
	[Range(1f, 10f)]
	private float Distortion = 1f;

	// Token: 0x040007C7 RID: 1991
	[Range(1f, 15f)]
	public int Size = 5;

	// Token: 0x040007C8 RID: 1992
	private Material SCMaterial;
}
