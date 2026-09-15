using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Noise")]
public class CameraFilterPack_Color_Noise : MonoBehaviour
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600027F RID: 639 RVA: 0x0000E9D6 File Offset: 0x0000CBD6
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

	// Token: 0x06000280 RID: 640 RVA: 0x0000EA0A File Offset: 0x0000CC0A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Noise");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000EA2C File Offset: 0x0000CC2C
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
			this.material.SetFloat("_Noise", this.Noise);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000EAE2 File Offset: 0x0000CCE2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003CB RID: 971
	public Shader SCShader;

	// Token: 0x040003CC RID: 972
	private float TimeX = 1f;

	// Token: 0x040003CD RID: 973
	private Vector4 ScreenResolution;

	// Token: 0x040003CE RID: 974
	private Material SCMaterial;

	// Token: 0x040003CF RID: 975
	[Range(0f, 1f)]
	public float Noise = 0.235f;
}
