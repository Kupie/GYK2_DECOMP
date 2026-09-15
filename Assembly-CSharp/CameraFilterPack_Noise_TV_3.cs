using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Noise/TV_3")]
public class CameraFilterPack_Noise_TV_3 : MonoBehaviour
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x00019E55 File Offset: 0x00018055
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

	// Token: 0x0600053F RID: 1343 RVA: 0x00019E89 File Offset: 0x00018089
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_Noise3") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Noise_TV_3");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00019EC0 File Offset: 0x000180C0
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

	// Token: 0x06000541 RID: 1345 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00019FCE File Offset: 0x000181CE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006EB RID: 1771
	public Shader SCShader;

	// Token: 0x040006EC RID: 1772
	private float TimeX = 1f;

	// Token: 0x040006ED RID: 1773
	private Vector4 ScreenResolution;

	// Token: 0x040006EE RID: 1774
	private Material SCMaterial;

	// Token: 0x040006EF RID: 1775
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040006F0 RID: 1776
	[Range(0f, 1f)]
	public float Fade_Additive;

	// Token: 0x040006F1 RID: 1777
	[Range(0f, 1f)]
	public float Fade_Distortion;

	// Token: 0x040006F2 RID: 1778
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x040006F3 RID: 1779
	private Texture2D Texture2;
}
