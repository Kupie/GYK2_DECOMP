using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 4")]
public class CameraFilterPack_NightVision_4 : MonoBehaviour
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x000197E2 File Offset: 0x000179E2
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

	// Token: 0x0600052B RID: 1323 RVA: 0x00019816 File Offset: 0x00017A16
	private void ChangeFilters()
	{
		this.Matrix9 = new float[]
		{
			200f, -200f, -200f, 195f, 4f, -160f, 200f, -200f, -200f, -200f,
			10f, -200f
		};
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00019830 File Offset: 0x00017A30
	private void Start()
	{
		this.ChangeFilters();
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00019858 File Offset: 0x00017A58
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
			this.material.SetFloat("_Red_R", this.Matrix9[0] / 100f);
			this.material.SetFloat("_Red_G", this.Matrix9[1] / 100f);
			this.material.SetFloat("_Red_B", this.Matrix9[2] / 100f);
			this.material.SetFloat("_Green_R", this.Matrix9[3] / 100f);
			this.material.SetFloat("_Green_G", this.Matrix9[4] / 100f);
			this.material.SetFloat("_Green_B", this.Matrix9[5] / 100f);
			this.material.SetFloat("_Blue_R", this.Matrix9[6] / 100f);
			this.material.SetFloat("_Blue_G", this.Matrix9[7] / 100f);
			this.material.SetFloat("_Blue_B", this.Matrix9[8] / 100f);
			this.material.SetFloat("_Red_C", this.Matrix9[9] / 100f);
			this.material.SetFloat("_Green_C", this.Matrix9[10] / 100f);
			this.material.SetFloat("_Blue_C", this.Matrix9[11] / 100f);
			this.material.SetFloat("_FadeFX", this.FadeFX);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00019A79 File Offset: 0x00017C79
	private void OnValidate()
	{
		this.ChangeFilters();
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00019A81 File Offset: 0x00017C81
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006D2 RID: 1746
	private string ShaderName = "CameraFilterPack/NightVision_4";

	// Token: 0x040006D3 RID: 1747
	public Shader SCShader;

	// Token: 0x040006D4 RID: 1748
	[Range(0f, 1f)]
	public float FadeFX = 1f;

	// Token: 0x040006D5 RID: 1749
	private float TimeX = 1f;

	// Token: 0x040006D6 RID: 1750
	private Vector4 ScreenResolution;

	// Token: 0x040006D7 RID: 1751
	private Material SCMaterial;

	// Token: 0x040006D8 RID: 1752
	private float[] Matrix9;
}
