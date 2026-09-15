using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/GrayScale")]
public class CameraFilterPack_Color_GrayScale : MonoBehaviour
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000273 RID: 627 RVA: 0x0000E74E File Offset: 0x0000C94E
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

	// Token: 0x06000274 RID: 628 RVA: 0x0000E782 File Offset: 0x0000C982
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_GrayScale");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
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

	// Token: 0x06000276 RID: 630 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000E85A File Offset: 0x0000CA5A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003C1 RID: 961
	public Shader SCShader;

	// Token: 0x040003C2 RID: 962
	private float TimeX = 1f;

	// Token: 0x040003C3 RID: 963
	[Range(0f, 1f)]
	public float _Fade = 1f;

	// Token: 0x040003C4 RID: 964
	private Vector4 ScreenResolution;

	// Token: 0x040003C5 RID: 965
	private Material SCMaterial;
}
