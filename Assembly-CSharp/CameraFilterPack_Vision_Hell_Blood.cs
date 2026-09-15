using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Hell_Blood")]
public class CameraFilterPack_Vision_Hell_Blood : MonoBehaviour
{
	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001F951 File Offset: 0x0001DB51
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

	// Token: 0x060006A0 RID: 1696 RVA: 0x0001F985 File Offset: 0x0001DB85
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Hell_Blood");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
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
			this.material.SetFloat("_Value", this.Hole_Size);
			this.material.SetFloat("_Value2", this.Hole_Smooth);
			this.material.SetFloat("_Value3", this.Hole_Speed * 15f);
			this.material.SetFloat("_Value4", this.Intensity);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001FAA6 File Offset: 0x0001DCA6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000878 RID: 2168
	public Shader SCShader;

	// Token: 0x04000879 RID: 2169
	private float TimeX = 1f;

	// Token: 0x0400087A RID: 2170
	private Vector4 ScreenResolution;

	// Token: 0x0400087B RID: 2171
	private Material SCMaterial;

	// Token: 0x0400087C RID: 2172
	[Range(0f, 1f)]
	public float Hole_Size = 0.57f;

	// Token: 0x0400087D RID: 2173
	[Range(0f, 0.5f)]
	public float Hole_Smooth = 0.362f;

	// Token: 0x0400087E RID: 2174
	[Range(-2f, 2f)]
	public float Hole_Speed = 0.85f;

	// Token: 0x0400087F RID: 2175
	[Range(-10f, 10f)]
	public float Intensity = 0.24f;
}
