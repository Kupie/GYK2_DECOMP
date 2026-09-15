using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 5")]
public class CameraFilterPack_Oculus_NightVision5 : MonoBehaviour
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001A5BB File Offset: 0x000187BB
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

	// Token: 0x06000559 RID: 1369 RVA: 0x0001A5EF File Offset: 0x000187EF
	private void ChangeFilters()
	{
		this.Matrix9 = new float[]
		{
			200f, -200f, -200f, 195f, 4f, -160f, 200f, -200f, -200f, -200f,
			10f, -200f
		};
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001A609 File Offset: 0x00018809
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

	// Token: 0x0600055B RID: 1371 RVA: 0x0001A634 File Offset: 0x00018834
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
			this.material.SetFloat("_Size", this._Size);
			this.material.SetFloat("_Dist", this._Dist);
			this.material.SetFloat("_Smooth", this._Smooth);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0001A897 File Offset: 0x00018A97
	private void OnValidate()
	{
		this.ChangeFilters();
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001A89F File Offset: 0x00018A9F
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000707 RID: 1799
	private string ShaderName = "CameraFilterPack/Oculus_NightVision5";

	// Token: 0x04000708 RID: 1800
	public Shader SCShader;

	// Token: 0x04000709 RID: 1801
	[Range(0f, 1f)]
	public float FadeFX = 1f;

	// Token: 0x0400070A RID: 1802
	[Range(0f, 1f)]
	public float _Size = 0.37f;

	// Token: 0x0400070B RID: 1803
	[Range(0f, 1f)]
	public float _Smooth = 0.15f;

	// Token: 0x0400070C RID: 1804
	[Range(0f, 1f)]
	public float _Dist = 0.285f;

	// Token: 0x0400070D RID: 1805
	private float TimeX = 1f;

	// Token: 0x0400070E RID: 1806
	private Vector4 ScreenResolution;

	// Token: 0x0400070F RID: 1807
	private Material SCMaterial;

	// Token: 0x04000710 RID: 1808
	private float[] Matrix9;
}
