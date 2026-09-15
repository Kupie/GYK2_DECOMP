using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Glitch1")]
public class CameraFilterPack_FX_Glitch1 : MonoBehaviour
{
	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000436 RID: 1078 RVA: 0x00015586 File Offset: 0x00013786
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

	// Token: 0x06000437 RID: 1079 RVA: 0x000155BA File Offset: 0x000137BA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Glitch1");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x000155DC File Offset: 0x000137DC
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
			this.material.SetFloat("_Glitch", this.Glitch);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00015692 File Offset: 0x00013892
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005B9 RID: 1465
	public Shader SCShader;

	// Token: 0x040005BA RID: 1466
	private float TimeX = 1f;

	// Token: 0x040005BB RID: 1467
	private Vector4 ScreenResolution;

	// Token: 0x040005BC RID: 1468
	private Material SCMaterial;

	// Token: 0x040005BD RID: 1469
	[Range(0f, 1f)]
	public float Glitch = 1f;
}
