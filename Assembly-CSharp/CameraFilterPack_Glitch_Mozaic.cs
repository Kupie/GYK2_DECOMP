using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Mozaic")]
public class CameraFilterPack_Glitch_Mozaic : MonoBehaviour
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x00016A4A File Offset: 0x00014C4A
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

	// Token: 0x06000497 RID: 1175 RVA: 0x00016A7E File Offset: 0x00014C7E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Glitch_Mozaic");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00016AA0 File Offset: 0x00014CA0
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
			this.material.SetFloat("_Value", this.Intensity);
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00016B98 File Offset: 0x00014D98
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400060F RID: 1551
	public Shader SCShader;

	// Token: 0x04000610 RID: 1552
	private float TimeX = 1f;

	// Token: 0x04000611 RID: 1553
	private Vector4 ScreenResolution;

	// Token: 0x04000612 RID: 1554
	private Material SCMaterial;

	// Token: 0x04000613 RID: 1555
	[Range(0.001f, 10f)]
	public float Intensity = 1f;

	// Token: 0x04000614 RID: 1556
	[Range(0f, 10f)]
	private float Value2 = 1f;

	// Token: 0x04000615 RID: 1557
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x04000616 RID: 1558
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
