using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Ascii")]
public class CameraFilterPack_FX_Ascii : MonoBehaviour
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x000146C3 File Offset: 0x000128C3
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

	// Token: 0x06000401 RID: 1025 RVA: 0x000146F7 File Offset: 0x000128F7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Ascii");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00014718 File Offset: 0x00012918
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
			this.material.SetFloat("Fade", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x000147E4 File Offset: 0x000129E4
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000571 RID: 1393
	public Shader SCShader;

	// Token: 0x04000572 RID: 1394
	[Range(0f, 2f)]
	public float Value = 1f;

	// Token: 0x04000573 RID: 1395
	[Range(0.01f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000574 RID: 1396
	private float TimeX = 1f;

	// Token: 0x04000575 RID: 1397
	private Vector4 ScreenResolution;

	// Token: 0x04000576 RID: 1398
	private Material SCMaterial;
}
