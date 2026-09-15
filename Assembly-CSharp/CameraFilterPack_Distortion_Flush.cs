using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Flush")]
public class CameraFilterPack_Distortion_Flush : MonoBehaviour
{
	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060002CD RID: 717 RVA: 0x0000FB93 File Offset: 0x0000DD93
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

	// Token: 0x060002CE RID: 718 RVA: 0x0000FBC7 File Offset: 0x0000DDC7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Flush");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000FBE8 File Offset: 0x0000DDE8
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
			this.material.SetFloat("Value", this.Value);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000FC9E File Offset: 0x0000DE9E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000419 RID: 1049
	public Shader SCShader;

	// Token: 0x0400041A RID: 1050
	private float TimeX = 1f;

	// Token: 0x0400041B RID: 1051
	private Vector4 ScreenResolution;

	// Token: 0x0400041C RID: 1052
	private Material SCMaterial;

	// Token: 0x0400041D RID: 1053
	[Range(-10f, 50f)]
	public float Value = 5f;
}
