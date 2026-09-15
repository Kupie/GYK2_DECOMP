using System;
using UnityEngine;

// Token: 0x020000EA RID: 234
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/ARCADE")]
public class CameraFilterPack_TV_ARCADE : MonoBehaviour
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0001BDBA File Offset: 0x00019FBA
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

	// Token: 0x060005B6 RID: 1462 RVA: 0x0001BDEE File Offset: 0x00019FEE
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_ARCADE");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0001BE10 File Offset: 0x0001A010
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0001BEAD File Offset: 0x0001A0AD
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400076F RID: 1903
	public Shader SCShader;

	// Token: 0x04000770 RID: 1904
	private float TimeX = 1f;

	// Token: 0x04000771 RID: 1905
	private Vector4 ScreenResolution;

	// Token: 0x04000772 RID: 1906
	private Material SCMaterial;
}
