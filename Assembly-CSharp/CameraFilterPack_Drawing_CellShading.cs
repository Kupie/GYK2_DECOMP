using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/CellShading")]
public class CameraFilterPack_Drawing_CellShading : MonoBehaviour
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000315 RID: 789 RVA: 0x00010E21 File Offset: 0x0000F021
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

	// Token: 0x06000316 RID: 790 RVA: 0x00010E55 File Offset: 0x0000F055
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00010E78 File Offset: 0x0000F078
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
			this.material.SetFloat("_EdgeSize", this.EdgeSize);
			this.material.SetFloat("_ColorLevel", this.ColorLevel);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00010F3D File Offset: 0x0000F13D
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000471 RID: 1137
	public Shader SCShader;

	// Token: 0x04000472 RID: 1138
	private float TimeX = 1f;

	// Token: 0x04000473 RID: 1139
	private Vector4 ScreenResolution;

	// Token: 0x04000474 RID: 1140
	private Material SCMaterial;

	// Token: 0x04000475 RID: 1141
	[Range(0f, 1f)]
	public float EdgeSize = 0.1f;

	// Token: 0x04000476 RID: 1142
	[Range(0f, 10f)]
	public float ColorLevel = 4f;
}
