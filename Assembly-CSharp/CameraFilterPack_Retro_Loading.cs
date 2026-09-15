using System;
using UnityEngine;

// Token: 0x020000E5 RID: 229
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Retro/Loading")]
public class CameraFilterPack_Retro_Loading : MonoBehaviour
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001B6E1 File Offset: 0x000198E1
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

	// Token: 0x06000598 RID: 1432 RVA: 0x0001B715 File Offset: 0x00019915
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Retro_Loading");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0001B738 File Offset: 0x00019938
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0001B7EE File Offset: 0x000199EE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000752 RID: 1874
	public Shader SCShader;

	// Token: 0x04000753 RID: 1875
	private float TimeX = 1f;

	// Token: 0x04000754 RID: 1876
	private Vector4 ScreenResolution;

	// Token: 0x04000755 RID: 1877
	private Material SCMaterial;

	// Token: 0x04000756 RID: 1878
	[Range(0.1f, 10f)]
	public float Speed = 1f;
}
