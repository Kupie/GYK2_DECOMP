using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Light/Rainbow")]
public class CameraFilterPack_Light_Rainbow : MonoBehaviour
{
	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00017F9E File Offset: 0x0001619E
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

	// Token: 0x060004E5 RID: 1253 RVA: 0x00017FD2 File Offset: 0x000161D2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Light_Rainbow");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00017FF4 File Offset: 0x000161F4
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x000180AA File Offset: 0x000162AA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000670 RID: 1648
	public Shader SCShader;

	// Token: 0x04000671 RID: 1649
	private float TimeX = 1f;

	// Token: 0x04000672 RID: 1650
	private Vector4 ScreenResolution;

	// Token: 0x04000673 RID: 1651
	private Material SCMaterial;

	// Token: 0x04000674 RID: 1652
	[Range(0.01f, 5f)]
	public float Value = 1.5f;
}
