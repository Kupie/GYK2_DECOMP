using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Lut/TestMode")]
public class CameraFilterPack_Lut_TestMode : MonoBehaviour
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600051A RID: 1306 RVA: 0x00018F3D File Offset: 0x0001713D
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

	// Token: 0x0600051B RID: 1307 RVA: 0x00018F71 File Offset: 0x00017171
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Lut_TestMode");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00018F94 File Offset: 0x00017194
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

	// Token: 0x0600051D RID: 1309 RVA: 0x00018694 File Offset: 0x00016894
	public bool ValidDimensions(Texture2D tex2d)
	{
		return tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001906C File Offset: 0x0001726C
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

	// Token: 0x0600051F RID: 1311 RVA: 0x00019170 File Offset: 0x00017370
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
			this.material.SetFloat("_Extra3", this.TestMode);
			Graphics.Blit(sourceTexture, destTexture, this.material, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00002318 File Offset: 0x00000518
	private void OnValidate()
	{
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001928A File Offset: 0x0001748A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006A4 RID: 1700
	public Shader SCShader;

	// Token: 0x040006A5 RID: 1701
	private float TimeX = 1f;

	// Token: 0x040006A6 RID: 1702
	private Vector4 ScreenResolution;

	// Token: 0x040006A7 RID: 1703
	private Material SCMaterial;

	// Token: 0x040006A8 RID: 1704
	public Texture2D LutTexture;

	// Token: 0x040006A9 RID: 1705
	private Texture3D converted3DLut;

	// Token: 0x040006AA RID: 1706
	[Range(0f, 1f)]
	public float Blend = 1f;

	// Token: 0x040006AB RID: 1707
	[Range(0f, 3f)]
	public float OriginalIntensity = 1f;

	// Token: 0x040006AC RID: 1708
	[Range(-1f, 1f)]
	public float ResultIntensity;

	// Token: 0x040006AD RID: 1709
	[Range(-1f, 1f)]
	public float FinalIntensity;

	// Token: 0x040006AE RID: 1710
	[Range(0f, 1f)]
	public float TestMode = 0.5f;

	// Token: 0x040006AF RID: 1711
	private string MemoPath;
}
