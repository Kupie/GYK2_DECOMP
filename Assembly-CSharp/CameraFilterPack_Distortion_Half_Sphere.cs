using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Half_Sphere")]
public class CameraFilterPack_Distortion_Half_Sphere : MonoBehaviour
{
	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000FCD6 File Offset: 0x0000DED6
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

	// Token: 0x060002D4 RID: 724 RVA: 0x0000FD0A File Offset: 0x0000DF0A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Half_Sphere");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000FD2C File Offset: 0x0000DF2C
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
			this.material.SetFloat("_SphereSize", this.SphereSize);
			this.material.SetFloat("_SpherePositionX", this.SpherePositionX);
			this.material.SetFloat("_SpherePositionY", this.SpherePositionY);
			this.material.SetFloat("_Strength", this.Strength);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000FE1D File Offset: 0x0000E01D
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400041E RID: 1054
	public Shader SCShader;

	// Token: 0x0400041F RID: 1055
	private float TimeX = 1f;

	// Token: 0x04000420 RID: 1056
	[Range(1f, 6f)]
	private Vector4 ScreenResolution;

	// Token: 0x04000421 RID: 1057
	private Material SCMaterial;

	// Token: 0x04000422 RID: 1058
	public float SphereSize = 2.5f;

	// Token: 0x04000423 RID: 1059
	[Range(-1f, 1f)]
	public float SpherePositionX;

	// Token: 0x04000424 RID: 1060
	[Range(-1f, 1f)]
	public float SpherePositionY;

	// Token: 0x04000425 RID: 1061
	[Range(1f, 10f)]
	public float Strength = 5f;
}
