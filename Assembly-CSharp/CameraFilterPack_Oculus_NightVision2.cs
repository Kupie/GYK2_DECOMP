using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 2")]
public class CameraFilterPack_Oculus_NightVision2 : MonoBehaviour
{
	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001A198 File Offset: 0x00018398
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

	// Token: 0x0600054B RID: 1355 RVA: 0x0001A1CC File Offset: 0x000183CC
	private void ChangeFilters()
	{
		this.Matrix9 = new float[]
		{
			200f, -200f, -200f, 195f, 4f, -160f, 200f, -200f, -200f, -200f,
			10f, -200f
		};
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001A1E6 File Offset: 0x000183E6
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

	// Token: 0x0600054D RID: 1357 RVA: 0x0001A210 File Offset: 0x00018410
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

	// Token: 0x0600054E RID: 1358 RVA: 0x0001A431 File Offset: 0x00018631
	private void OnValidate()
	{
		this.ChangeFilters();
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001A439 File Offset: 0x00018639
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006FB RID: 1787
	private string ShaderName = "CameraFilterPack/Oculus_NightVision2";

	// Token: 0x040006FC RID: 1788
	public Shader SCShader;

	// Token: 0x040006FD RID: 1789
	[Range(0f, 1f)]
	public float FadeFX = 1f;

	// Token: 0x040006FE RID: 1790
	private float TimeX = 1f;

	// Token: 0x040006FF RID: 1791
	private Vector4 ScreenResolution;

	// Token: 0x04000700 RID: 1792
	private Material SCMaterial;

	// Token: 0x04000701 RID: 1793
	private float[] Matrix9;
}
