using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Lens")]
public class CameraFilterPack_Distortion_Lens : MonoBehaviour
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060002DF RID: 735 RVA: 0x0000FFA6 File Offset: 0x0000E1A6
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

	// Token: 0x060002E0 RID: 736 RVA: 0x0000FFDA File Offset: 0x0000E1DA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Lens");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000FFFC File Offset: 0x0000E1FC
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
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000100D7 File Offset: 0x0000E2D7
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400042B RID: 1067
	public Shader SCShader;

	// Token: 0x0400042C RID: 1068
	private float TimeX = 1f;

	// Token: 0x0400042D RID: 1069
	private Vector4 ScreenResolution;

	// Token: 0x0400042E RID: 1070
	private Material SCMaterial;

	// Token: 0x0400042F RID: 1071
	[Range(-1f, 1f)]
	public float CenterX;

	// Token: 0x04000430 RID: 1072
	[Range(-1f, 1f)]
	public float CenterY;

	// Token: 0x04000431 RID: 1073
	[Range(0f, 3f)]
	public float Distortion = 1f;
}
