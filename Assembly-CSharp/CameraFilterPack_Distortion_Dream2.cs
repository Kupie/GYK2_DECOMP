using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Dream2")]
public class CameraFilterPack_Distortion_Dream2 : MonoBehaviour
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060002BB RID: 699 RVA: 0x0000F7AE File Offset: 0x0000D9AE
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

	// Token: 0x060002BC RID: 700 RVA: 0x0000F7E2 File Offset: 0x0000D9E2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Dream2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000F804 File Offset: 0x0000DA04
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
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000408 RID: 1032
	public Shader SCShader;

	// Token: 0x04000409 RID: 1033
	private float TimeX = 1f;

	// Token: 0x0400040A RID: 1034
	private Vector4 ScreenResolution;

	// Token: 0x0400040B RID: 1035
	private Material SCMaterial;

	// Token: 0x0400040C RID: 1036
	[Range(0f, 100f)]
	public float Distortion = 6f;

	// Token: 0x0400040D RID: 1037
	[Range(0f, 32f)]
	public float Speed = 5f;
}
