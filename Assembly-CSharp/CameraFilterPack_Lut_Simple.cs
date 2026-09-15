using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Lut/Simple")]
public class CameraFilterPack_Lut_Simple : MonoBehaviour
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000510 RID: 1296 RVA: 0x00018C32 File Offset: 0x00016E32
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

	// Token: 0x06000511 RID: 1297 RVA: 0x00018C66 File Offset: 0x00016E66
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Lut_Simple");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00018C88 File Offset: 0x00016E88
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

	// Token: 0x06000513 RID: 1299 RVA: 0x00018694 File Offset: 0x00016894
	public bool ValidDimensions(Texture2D tex2d)
	{
		return tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00018D60 File Offset: 0x00016F60
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

	// Token: 0x06000515 RID: 1301 RVA: 0x00018E64 File Offset: 0x00017064
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
			Graphics.Blit(sourceTexture, destTexture, this.material, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00002318 File Offset: 0x00000518
	private void OnValidate()
	{
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00018F10 File Offset: 0x00017110
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400069D RID: 1693
	public Shader SCShader;

	// Token: 0x0400069E RID: 1694
	private float TimeX = 1f;

	// Token: 0x0400069F RID: 1695
	private Vector4 ScreenResolution;

	// Token: 0x040006A0 RID: 1696
	private Material SCMaterial;

	// Token: 0x040006A1 RID: 1697
	public Texture2D LutTexture;

	// Token: 0x040006A2 RID: 1698
	private Texture3D converted3DLut;

	// Token: 0x040006A3 RID: 1699
	private string MemoPath;
}
