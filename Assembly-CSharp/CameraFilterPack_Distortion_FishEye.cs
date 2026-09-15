using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/FishEye")]
public class CameraFilterPack_Distortion_FishEye : MonoBehaviour
{
	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000F913 File Offset: 0x0000DB13
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

	// Token: 0x060002C2 RID: 706 RVA: 0x0000F947 File Offset: 0x0000DB47
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_FishEye");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0000F968 File Offset: 0x0000DB68
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000FA1E File Offset: 0x0000DC1E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400040E RID: 1038
	public Shader SCShader;

	// Token: 0x0400040F RID: 1039
	private float TimeX = 1f;

	// Token: 0x04000410 RID: 1040
	private Vector4 ScreenResolution;

	// Token: 0x04000411 RID: 1041
	private Material SCMaterial;

	// Token: 0x04000412 RID: 1042
	[Range(0f, 1f)]
	public float Distortion = 0.35f;
}
