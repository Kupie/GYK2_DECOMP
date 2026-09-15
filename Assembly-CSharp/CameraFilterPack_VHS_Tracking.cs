using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/Tracking")]
public class CameraFilterPack_VHS_Tracking : MonoBehaviour
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001EDA5 File Offset: 0x0001CFA5
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

	// Token: 0x06000676 RID: 1654 RVA: 0x0001EDD9 File Offset: 0x0001CFD9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/VHS_Tracking");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0001EDFC File Offset: 0x0001CFFC
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
			this.material.SetFloat("_Value", this.Tracking);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0001EEB2 File Offset: 0x0001D0B2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000841 RID: 2113
	public Shader SCShader;

	// Token: 0x04000842 RID: 2114
	private float TimeX = 1f;

	// Token: 0x04000843 RID: 2115
	private Vector4 ScreenResolution;

	// Token: 0x04000844 RID: 2116
	private Material SCMaterial;

	// Token: 0x04000845 RID: 2117
	[Range(0f, 2f)]
	public float Tracking = 1f;
}
