using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Invert")]
public class CameraFilterPack_Color_Invert : MonoBehaviour
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000279 RID: 633 RVA: 0x0000E892 File Offset: 0x0000CA92
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

	// Token: 0x0600027A RID: 634 RVA: 0x0000E8C6 File Offset: 0x0000CAC6
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Invert");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
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
			this.material.SetFloat("_Fade", this._Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000E99E File Offset: 0x0000CB9E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003C6 RID: 966
	public Shader SCShader;

	// Token: 0x040003C7 RID: 967
	private float TimeX = 1f;

	// Token: 0x040003C8 RID: 968
	[Range(0f, 1f)]
	public float _Fade = 1f;

	// Token: 0x040003C9 RID: 969
	private Vector4 ScreenResolution;

	// Token: 0x040003CA RID: 970
	private Material SCMaterial;
}
