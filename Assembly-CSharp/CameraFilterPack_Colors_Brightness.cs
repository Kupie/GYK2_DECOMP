using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Brightness")]
public class CameraFilterPack_Colors_Brightness : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000231 RID: 561 RVA: 0x0000D85E File Offset: 0x0000BA5E
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

	// Token: 0x06000232 RID: 562 RVA: 0x0000D892 File Offset: 0x0000BA92
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Brightness");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000D8B3 File Offset: 0x0000BAB3
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetFloat("_Val", this._Brightness);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000D8EE File Offset: 0x0000BAEE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400037C RID: 892
	public Shader SCShader;

	// Token: 0x0400037D RID: 893
	[Range(0f, 2f)]
	public float _Brightness = 1.5f;

	// Token: 0x0400037E RID: 894
	private Material SCMaterial;
}
