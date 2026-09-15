using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Glitch3")]
public class CameraFilterPack_FX_Glitch3 : MonoBehaviour
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x0001580E File Offset: 0x00013A0E
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

	// Token: 0x06000443 RID: 1091 RVA: 0x00015842 File Offset: 0x00013A42
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Glitch3");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00015864 File Offset: 0x00013A64
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
			this.material.SetFloat("_Glitch", this._Glitch);
			this.material.SetFloat("_Noise", this._Noise);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00015930 File Offset: 0x00013B30
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005C3 RID: 1475
	public Shader SCShader;

	// Token: 0x040005C4 RID: 1476
	private float TimeX = 1f;

	// Token: 0x040005C5 RID: 1477
	private Vector4 ScreenResolution;

	// Token: 0x040005C6 RID: 1478
	private Material SCMaterial;

	// Token: 0x040005C7 RID: 1479
	[Range(0f, 1f)]
	public float _Glitch = 1f;

	// Token: 0x040005C8 RID: 1480
	[Range(0f, 1f)]
	public float _Noise = 1f;
}
