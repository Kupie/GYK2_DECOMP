using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Posterize")]
public class CameraFilterPack_TV_Posterize : MonoBehaviour
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001DA06 File Offset: 0x0001BC06
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

	// Token: 0x06000622 RID: 1570 RVA: 0x0001DA3A File Offset: 0x0001BC3A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Posterize");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0001DA5C File Offset: 0x0001BC5C
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
			this.material.SetFloat("_Distortion", this.Posterize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001DAE2 File Offset: 0x0001BCE2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007E9 RID: 2025
	public Shader SCShader;

	// Token: 0x040007EA RID: 2026
	private float TimeX = 1f;

	// Token: 0x040007EB RID: 2027
	[Range(1f, 256f)]
	public float Posterize = 64f;

	// Token: 0x040007EC RID: 2028
	private Material SCMaterial;
}
