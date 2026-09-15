using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Plasma")]
public class CameraFilterPack_FX_Plasma : MonoBehaviour
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x000160C6 File Offset: 0x000142C6
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

	// Token: 0x0600046D RID: 1133 RVA: 0x000160FA File Offset: 0x000142FA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Plasma");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001611C File Offset: 0x0001431C
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

	// Token: 0x0600046F RID: 1135 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000161D2 File Offset: 0x000143D2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005E7 RID: 1511
	public Shader SCShader;

	// Token: 0x040005E8 RID: 1512
	private float TimeX = 1f;

	// Token: 0x040005E9 RID: 1513
	private Vector4 ScreenResolution;

	// Token: 0x040005EA RID: 1514
	private Material SCMaterial;

	// Token: 0x040005EB RID: 1515
	[Range(0f, 20f)]
	private float Value = 6f;
}
