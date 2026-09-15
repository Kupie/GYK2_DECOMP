using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/NewCellShading")]
public class CameraFilterPack_Drawing_NewCellShading : MonoBehaviour
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000381 RID: 897 RVA: 0x000126DC File Offset: 0x000108DC
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

	// Token: 0x06000382 RID: 898 RVA: 0x00012710 File Offset: 0x00010910
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_NewCellShading");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00012734 File Offset: 0x00010934
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
			this.material.SetFloat("_Threshold", this.Threshold);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000127E3 File Offset: 0x000109E3
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004DD RID: 1245
	public Shader SCShader;

	// Token: 0x040004DE RID: 1246
	private float TimeX = 1f;

	// Token: 0x040004DF RID: 1247
	private Vector4 ScreenResolution;

	// Token: 0x040004E0 RID: 1248
	private Material SCMaterial;

	// Token: 0x040004E1 RID: 1249
	[Range(0f, 1f)]
	public float Threshold = 0.2f;
}
