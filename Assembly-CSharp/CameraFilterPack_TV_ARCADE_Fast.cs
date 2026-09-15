using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/ARCADE_Fast")]
public class CameraFilterPack_TV_ARCADE_Fast : MonoBehaviour
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001C081 File Offset: 0x0001A281
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

	// Token: 0x060005C2 RID: 1474 RVA: 0x0001C0B5 File Offset: 0x0001A2B5
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_Arcade1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/TV_ARCADE_Fast");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0001C0EC File Offset: 0x0001A2EC
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
			this.material.SetFloat("_Value", this.Interferance_Size);
			this.material.SetFloat("_Value2", this.Interferance_Speed);
			this.material.SetFloat("_Value3", this.Contrast);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0001C1FA File Offset: 0x0001A3FA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400077B RID: 1915
	public Shader SCShader;

	// Token: 0x0400077C RID: 1916
	private float TimeX = 1f;

	// Token: 0x0400077D RID: 1917
	private Vector4 ScreenResolution;

	// Token: 0x0400077E RID: 1918
	private Material SCMaterial;

	// Token: 0x0400077F RID: 1919
	[Range(0f, 0.05f)]
	public float Interferance_Size = 0.02f;

	// Token: 0x04000780 RID: 1920
	[Range(0f, 4f)]
	public float Interferance_Speed = 0.5f;

	// Token: 0x04000781 RID: 1921
	[Range(0f, 10f)]
	public float Contrast = 1f;

	// Token: 0x04000782 RID: 1922
	[Range(0f, 10f)]
	private float Value4 = 1f;

	// Token: 0x04000783 RID: 1923
	private Texture2D Texture2;
}
