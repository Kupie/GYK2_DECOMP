using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/BleachBypass")]
public class CameraFilterPack_Colors_BleachBypass : MonoBehaviour
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600022B RID: 555 RVA: 0x0000D718 File Offset: 0x0000B918
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

	// Token: 0x0600022C RID: 556 RVA: 0x0000D74C File Offset: 0x0000B94C
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_BleachBypass");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000D770 File Offset: 0x0000B970
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

	// Token: 0x0600022E RID: 558 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000D826 File Offset: 0x0000BA26
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000377 RID: 887
	public Shader SCShader;

	// Token: 0x04000378 RID: 888
	private float TimeX = 1f;

	// Token: 0x04000379 RID: 889
	private Vector4 ScreenResolution;

	// Token: 0x0400037A RID: 890
	private Material SCMaterial;

	// Token: 0x0400037B RID: 891
	[Range(-1f, 2f)]
	public float Value = 1f;
}
