using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Lut/PlayWith")]
public class CameraFilterPack_Lut_PlayWith : MonoBehaviour
{
	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x00018565 File Offset: 0x00016765
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

	// Token: 0x060004FD RID: 1277 RVA: 0x00018599 File Offset: 0x00016799
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Lut_PlayWith");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000185BC File Offset: 0x000167BC
	public void SetIdentityLut()
	{
		int num = 16;
		Color[] array = new Color[num * num * num];
		float num2 = 1f / (1f * (float)num - 1f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
				}
			}
		}
		if (this.converted3DLut)
		{
			global::UnityEngine.Object.DestroyImmediate(this.converted3DLut);
		}
		this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
		this.converted3DLut.SetPixels(array);
		this.converted3DLut.Apply();
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00018694 File Offset: 0x00016894
	public bool ValidDimensions(Texture2D tex2d)
	{
		return tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000186BC File Offset: 0x000168BC
	public void Convert(Texture2D temp2DTex)
	{
		if (!temp2DTex)
		{
			this.SetIdentityLut();
			return;
		}
		int num = temp2DTex.width * temp2DTex.height;
		num = temp2DTex.height;
		if (!this.ValidDimensions(temp2DTex))
		{
			Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
			return;
		}
		Color[] pixels = temp2DTex.GetPixels();
		Color[] array = new Color[pixels.Length];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					int num2 = num - j - 1;
					array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
				}
			}
		}
		if (this.converted3DLut)
		{
			global::UnityEngine.Object.DestroyImmediate(this.converted3DLut);
		}
		this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
		this.converted3DLut.SetPixels(array);
		this.converted3DLut.Apply();
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000187C0 File Offset: 0x000169C0
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null || !SystemInfo.supports3DTextures)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			if (this.converted3DLut == null)
			{
				this.Convert(this.LutTexture);
			}
			this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
			this.material.SetTexture("_LutTex", this.converted3DLut);
			this.material.SetFloat("_Blend", this.Blend);
			this.material.SetFloat("_Intensity", this.OriginalIntensity);
			this.material.SetFloat("_Extra", this.ResultIntensity);
			this.material.SetFloat("_Extra2", this.FinalIntensity);
			Graphics.Blit(sourceTexture, destTexture, this.material, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00002318 File Offset: 0x00000518
	private void OnValidate()
	{
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x000188C4 File Offset: 0x00016AC4
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400068A RID: 1674
	public Shader SCShader;

	// Token: 0x0400068B RID: 1675
	private float TimeX = 1f;

	// Token: 0x0400068C RID: 1676
	private Vector4 ScreenResolution;

	// Token: 0x0400068D RID: 1677
	private Material SCMaterial;

	// Token: 0x0400068E RID: 1678
	public Texture2D LutTexture;

	// Token: 0x0400068F RID: 1679
	private Texture3D converted3DLut;

	// Token: 0x04000690 RID: 1680
	[Range(0f, 1f)]
	public float Blend = 1f;

	// Token: 0x04000691 RID: 1681
	[Range(0f, 3f)]
	public float OriginalIntensity = 1f;

	// Token: 0x04000692 RID: 1682
	[Range(-1f, 1f)]
	public float ResultIntensity;

	// Token: 0x04000693 RID: 1683
	[Range(-1f, 1f)]
	public float FinalIntensity;

	// Token: 0x04000694 RID: 1684
	private string MemoPath;
}
