using System;
using UnityEngine;

// Token: 0x020000D6 RID: 214
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Noise/TV_2")]
public class CameraFilterPack_Noise_TV_2 : MonoBehaviour
{
	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x00019C97 File Offset: 0x00017E97
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

	// Token: 0x06000539 RID: 1337 RVA: 0x00019CCB File Offset: 0x00017ECB
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_Noise2") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Noise_TV_2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00019D04 File Offset: 0x00017F04
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
			this.material.SetFloat("_Value", this.Fade);
			this.material.SetFloat("_Value2", this.Fade_Additive);
			this.material.SetFloat("_Value3", this.Fade_Distortion);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00019E12 File Offset: 0x00018012
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006E2 RID: 1762
	public Shader SCShader;

	// Token: 0x040006E3 RID: 1763
	private float TimeX = 1f;

	// Token: 0x040006E4 RID: 1764
	private Vector4 ScreenResolution;

	// Token: 0x040006E5 RID: 1765
	private Material SCMaterial;

	// Token: 0x040006E6 RID: 1766
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040006E7 RID: 1767
	[Range(0f, 1f)]
	public float Fade_Additive;

	// Token: 0x040006E8 RID: 1768
	[Range(0f, 1f)]
	public float Fade_Distortion;

	// Token: 0x040006E9 RID: 1769
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x040006EA RID: 1770
	private Texture2D Texture2;
}
