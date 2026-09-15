using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/HSV")]
public class CameraFilterPack_Colors_HSV : MonoBehaviour
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600023D RID: 573 RVA: 0x0000DAC1 File Offset: 0x0000BCC1
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

	// Token: 0x0600023E RID: 574 RVA: 0x0000DAF5 File Offset: 0x0000BCF5
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000DB08 File Offset: 0x0000BD08
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetFloat("_HueShift", this._HueShift);
			this.material.SetFloat("_Sat", this._Saturation);
			this.material.SetFloat("_Val", this._ValueBrightness);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000DB7A File Offset: 0x0000BD7A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000387 RID: 903
	public Shader SCShader;

	// Token: 0x04000388 RID: 904
	[Range(0f, 360f)]
	public float _HueShift = 180f;

	// Token: 0x04000389 RID: 905
	[Range(-32f, 32f)]
	public float _Saturation = 1f;

	// Token: 0x0400038A RID: 906
	[Range(-32f, 32f)]
	public float _ValueBrightness = 1f;

	// Token: 0x0400038B RID: 907
	private Material SCMaterial;
}
