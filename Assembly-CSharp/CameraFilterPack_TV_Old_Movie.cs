using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Old_Movie")]
public class CameraFilterPack_TV_Old_Movie : MonoBehaviour
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001D5F2 File Offset: 0x0001B7F2
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

	// Token: 0x06000610 RID: 1552 RVA: 0x0001D626 File Offset: 0x0001B826
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Old_Movie");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0001D648 File Offset: 0x0001B848
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

	// Token: 0x06000612 RID: 1554 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0001D6CE File Offset: 0x0001B8CE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007D7 RID: 2007
	public Shader SCShader;

	// Token: 0x040007D8 RID: 2008
	private float TimeX = 1f;

	// Token: 0x040007D9 RID: 2009
	[Range(1f, 10f)]
	public float Distortion = 1f;

	// Token: 0x040007DA RID: 2010
	private Material SCMaterial;
}
