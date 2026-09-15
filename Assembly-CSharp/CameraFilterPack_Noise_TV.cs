using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Noise/TV")]
public class CameraFilterPack_Noise_TV : MonoBehaviour
{
	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000532 RID: 1330 RVA: 0x00019AC4 File Offset: 0x00017CC4
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

	// Token: 0x06000533 RID: 1331 RVA: 0x00019AF8 File Offset: 0x00017CF8
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_Noise") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Noise_TV");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00019B30 File Offset: 0x00017D30
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
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00019C3E File Offset: 0x00017E3E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006D9 RID: 1753
	public Shader SCShader;

	// Token: 0x040006DA RID: 1754
	private float TimeX = 1f;

	// Token: 0x040006DB RID: 1755
	private Vector4 ScreenResolution;

	// Token: 0x040006DC RID: 1756
	private Material SCMaterial;

	// Token: 0x040006DD RID: 1757
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x040006DE RID: 1758
	[Range(0f, 10f)]
	private float Value2 = 1f;

	// Token: 0x040006DF RID: 1759
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x040006E0 RID: 1760
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x040006E1 RID: 1761
	private Texture2D Texture2;
}
