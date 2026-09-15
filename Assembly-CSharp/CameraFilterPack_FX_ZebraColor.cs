using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/ZebraColor")]
public class CameraFilterPack_FX_ZebraColor : MonoBehaviour
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x00016906 File Offset: 0x00014B06
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

	// Token: 0x06000491 RID: 1169 RVA: 0x0001693A File Offset: 0x00014B3A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_ZebraColor");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0001695C File Offset: 0x00014B5C
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00016A12 File Offset: 0x00014C12
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400060A RID: 1546
	public Shader SCShader;

	// Token: 0x0400060B RID: 1547
	private float TimeX = 1f;

	// Token: 0x0400060C RID: 1548
	private Vector4 ScreenResolution;

	// Token: 0x0400060D RID: 1549
	private Material SCMaterial;

	// Token: 0x0400060E RID: 1550
	[Range(1f, 10f)]
	public float Value = 3f;
}
