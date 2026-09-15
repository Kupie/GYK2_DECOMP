using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Neon")]
public class CameraFilterPack_Edge_Neon : MonoBehaviour
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060003B1 RID: 945 RVA: 0x00013447 File Offset: 0x00011647
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

	// Token: 0x060003B2 RID: 946 RVA: 0x0001347B File Offset: 0x0001167B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Neon");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001349C File Offset: 0x0001169C
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
			this.material.SetFloat("_EdgeWeight", this.EdgeWeight);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001354B File Offset: 0x0001174B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400051D RID: 1309
	public Shader SCShader;

	// Token: 0x0400051E RID: 1310
	private float TimeX = 1f;

	// Token: 0x0400051F RID: 1311
	private Vector4 ScreenResolution;

	// Token: 0x04000520 RID: 1312
	private Material SCMaterial;

	// Token: 0x04000521 RID: 1313
	[Range(1f, 10f)]
	public float EdgeWeight = 1f;
}
